using AutoMapper;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Asn1.Ocsp;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Enum;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using System.Globalization;

namespace ProyectoMancariBlue.Controllers
{
    public class ReporteController : Controller
    {

        private readonly IReporteModel _reporteModel;
        private readonly IMapper _mapper;
        private readonly IVenta _venta;
        private readonly IAnimalModel _animal;



        public ReporteController(IReporteModel _ReporteModel, IMapper mapper, IVenta venta, IAnimalModel animal)
        {

            _reporteModel = _ReporteModel;
            _mapper = mapper;
            _venta = venta;
            _animal = animal;

        }
        [Authorize(Roles = "Admin,Contador")]
        public IActionResult Index()
        {
            try
            {
                ReporteRequest reporteRequest = new ReporteRequest();

                reporteRequest.listaTransacciones = _mapper.Map<IEnumerable<VentaDTO>>(_venta.GetAll().ToList());
                var reporte = _reporteModel.GetAllReporte();
                if (reporte == null)
                {
                    return NotFound();
                }

                reporteRequest.listaReporte = _mapper.Map<List<ReporteDTO>>(reporte);
                return View(reporteRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desplegar los reportes: {ex}");
                return View();
            }
        }



        public async Task<IActionResult> CrearReporte(ReporteDTO reporte)
        {
            try
            {
                string errors = "";

                if (!Validar(reporte, ref errors))
                {
                    return Json(new { success = false, message = errors });
                }

                if (reporte.Id == null)
                {
                    reporte.FechaCreacion = DateTime.Now;
                }

                await _reporteModel.CreateReporte(_mapper.Map<Reporte>(reporte));
                return Json(new { success = true, message = "Reporte creado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Ha ocurrido un error. " + ex.Message
                });
            }
        }



        public IActionResult EditarReporte()
        {
            return View();
        }

        public IActionResult VerReporteVenta()
        {

            var reportes = _reporteModel.GetVenta();


            var reporteDTOs = reportes.Select(r => _mapper.Map<ReporteDTO>(r)).ToList();


            return View(reporteDTOs);

        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var vacacion = await _reporteModel.GetReporteById(Id);
                await _reporteModel.DeleteReporteID(vacacion.Id);
                return Json(new { success = true, message = "Registro eliminado con éxito" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " });
            }

        }
        public async Task<IActionResult> ValidarEliminacion(int Id)
        {
            try
            {

                var vacacion = await _reporteModel.GetReporteById(Id);
                TimeSpan DiasTranscurridos = DateTime.Now - vacacion.FechaCreacion;

                if (DiasTranscurridos.Days > 0)
                {

                    return Json(new { success = false, message = "No es posible eliminar este registro debido a que ya ha transcurrido más de 1 días desde su creación." });
                }
                else
                {
                    return Json(new { success = true, message = "Operación validada", Data = vacacion });
                }
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " });
            }
        }

        private bool Validar(ReporteDTO reporte, ref string errors)
        {
            if (string.IsNullOrEmpty(reporte.CodigoCVO))
            {
                errors = "El código CVO es obligatorio. ";
                return false;
            }
            if (string.IsNullOrEmpty(reporte.CodigoTransporte))
            {
                errors += "El código de transporte es obligatorio. \n";
                return false;
            }
            if (string.IsNullOrEmpty(reporte.Transaccion.ToString()))
            {
                errors += "La Transaccion es obligatoria.  \n";
                return false;
            }
            if (string.IsNullOrEmpty(reporte.Identificacion))
            {
                errors += "La Identificacion es obligatoria. \n";
                return false;
            }
            //else if (reporte.Identificacion.Length > 8)
            //{
            //    errors += "La Identificacion no puede tener más de 8 dígitos. \n";
            //}
            //else if (!System.Text.RegularExpressions.Regex.IsMatch(reporte.Identificacion, @"^\d*$"))
            //{
            //    errors += "La Identificacion solo puede contener dígitos numéricos. \n";
            //}
            if (string.IsNullOrEmpty(reporte.NombreCliente))
            {
                errors += "El Nombre del Cliente es obligatorio. \n";
                return false;
            }
            if (string.IsNullOrEmpty(reporte.FechaCreacion.ToString()))
            {
                errors += "La FechaCreacion es obligatoria.  \n";
                return false;
            }


            return true;

        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAnimales(string idAnimal)
        {
            try
            {
                List<Animal> animales = new List<Animal>();
                var ids = idAnimal.Split(',');

                foreach (var id in ids)
                {
                    if (int.TryParse(id.Trim(), out int animalId))
                    {
                        var animal = await _animal.GetAnimalById(animalId);
                        {
                            animales.Add(animal);
                        }
                    }
                    else
                    {
                        return BadRequest($"El ID '{id}' no es válido.");
                    }
                }


                var AnimalDTO = _mapper.Map<List<AnimalDTO>>(animales);

                return PartialView("_PartialViewAnimalesReporte", AnimalDTO);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> GenerarPDFReporte (long RegistroId)
        {
            ReporteDTO registro = _mapper.Map<ReporteDTO>(await _reporteModel.GetReporteById(RegistroId));
            Venta venta = await _venta.GetByIdAsync(registro.Transaccion.Value);
        
            var listaAnimales = _animal.GetAnimalsByIds(venta.IdAnimal
                      .Split(',')
                      .Select(id => long.Parse(id))
                      .ToList());

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Información Ganado")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Codigo CVO: {registro.CodigoCVO}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Codigo Transporte: {registro.CodigoTransporte}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Identificación del cliente: {registro.Identificacion}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Nombre Cliente: {registro.NombreCliente}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Transacción: {registro.Transaccion}")
                    .SetFontSize(12));

                document.Add(new Paragraph(" "));

                float[] columnWidths = { 1, 2, 2 };
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100));

                Style headerStyle = new Style()
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold();

                Style dataStyle = new Style()
                    .SetTextAlignment(TextAlignment.CENTER);

                table.AddHeaderCell(new Cell().Add(new Paragraph("Id")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Código")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Raza")).AddStyle(headerStyle));

                foreach (var animal in listaAnimales)
                {
                    table.AddCell(new Cell().Add(new Paragraph(animal.Id.ToString())).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(animal.Codigo)).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(animal.Raza.GetDisplayName())).AddStyle(dataStyle));
                }

                document.Add(table);

                document.Close();
                var byteInfo = ms.ToArray();

                return File(byteInfo, "application/pdf", $"{registro.NombreCliente}_InformacionGanado.pdf");
            }
        }
        public async Task<IActionResult> Edit(ReporteDTO model)
        {

            var errors = "";
            //var reporte = await _reporteModel.GetReporteById(model.IdProducto.Value);
            if (Validar(model, ref errors))
            {
                var reporte = _mapper.Map<Reporte>(model);
                await _reporteModel.UpdateAsync(reporte);


                return Json(new { success = true, message = "Registro de reporte modificado exitosamente." });
            }
            return Json(new { success = false, errors });
        }

    }
}
