using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Enum;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using static ProyectoMancariBlue.Controllers.EmpleadoController;

namespace ProyectoMancariBlue.Controllers
{
    public class Transaccion : Controller
    {
        private TransaccionRequest request;
        private readonly ICompra _compra;
        private readonly IVenta _venta;
        private readonly IMapper _Mapper;
        private readonly IAnimalModel _animalModel;
        private readonly IProducto _producto;
        private readonly IProveedor _proveedor;
        private string Nombre { get; set; }
        public Transaccion(ICompra compra, IVenta venta, IMapper mapper, IAnimalModel animalModel, IProducto producto, IProveedor proveedor)
        {
            request = new TransaccionRequest();
            _compra = compra;
            _venta = venta;
            _Mapper = mapper;
            _animalModel = animalModel;
            _producto = producto;
            _proveedor = proveedor;
        }
        public string RetornarPalabra(string Nombre) {
          this. Nombre = "Brandom"; 


            return "";
        }
        [Authorize(Roles = "Admin,Contador")]
        public IActionResult Index()
        {
            var Lista = _compra.GetAll();
            request.CompraDTOs = _Mapper.Map<List<CompraDTO>>(_compra.GetAll());
            request.ventaDTOs = _Mapper.Map<List<VentaDTO>>(_venta.GetAll());
            request.proveedorDTOs = _Mapper.Map<List<ProveedorDTO>>(_proveedor.GetAll());
            request.VentaListaAnimal = _Mapper.Map<List<AnimalDTO>>(_animalModel.GetAnimal());
           

            return View(request);
        }
        public IActionResult CrearTransaccionC()
        {
            return View();
        }
        public IActionResult CrearTransaccionV()
        {
            return View();
        }

        public IActionResult EditarTransaccionC()
        {
            return View();
        }

        public IActionResult EditarTransaccionV()
        {
            return View();
        }

        public IActionResult VerTransaccionC()
        {
            return View();
        }

        public IActionResult VerTransaccionV()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAnimal(int tipo)
        {
            if (tipo == 3)
            {
                var lista = _animalModel.GetAnimal().Where(x => x.Estado).ToList();
                return Json(new { list = _Mapper.Map<List<AnimalDTO>>(lista), isProduct = false });
            }
            else
            {
                var lista = await _producto.GetAllAsync();
                return Json(new { list = _Mapper.Map<List<ProductoDto>>(lista.Where(x => (int)x.IdTipoProducto == tipo).ToList()), isProduct = true });
            }

        }
        [HttpGet]
        public async Task<IActionResult> UpdateAnimalVenta()
        {
           
                var lista = _animalModel.GetAnimal().Where(x => x.Estado).ToList();
                return Json(new { list = _Mapper.Map<List<AnimalDTO>>(lista), isProduct = false });
            

        }
        [HttpPost]
        public async Task<IActionResult> CrearCompra(CompraDTO compra)
        {
            string errors = "";
            if (!Validar(compra, ref errors))
            {

                return Json(new { success=false, message = errors });
            }
            
            if(compra.IdTipoProducto.Equals(EProductType.Animales)) { compra.Cantidad = 1; }
            compra.FechaCreacion = DateTime.Now;
            var respuesta = await _compra.PostCompra(_Mapper.Map<Compra>(compra));
            if (respuesta != null)
            {

                return Json(new { success = true, message = "Registro de compra creado exitosamente." });
            }
            else
            {
                return Json(new { success = false, errors = "Ha ocurrido un error al crear el registro." });
            }

        }
        [HttpPost]
        public async Task<IActionResult> CrearVenta(VentaDTO venta)
        {
            try
            {
                string errors = "";
                if (!ValidarVenta(venta, ref errors))
                {

                    return Json(new { success = false, message = errors });
                }
                venta.FechaCreacion=DateTime.Now;
                var respuesta = await _venta.PostVenta(_Mapper.Map<Venta>(venta));
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de compra creado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, errors = "Ha ocurrido un error al crear el registro." });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
         

        }
        [HttpPost]
        public async Task<IActionResult> ModificarVenta(VentaDTO venta)
        {
            try
            {
                string errors = "";
                if (!ValidarVenta(venta, ref errors))
                {

                    return Json(new { success = false, message = errors });
                }
                var respuesta = await _venta.UpdateAsync(_Mapper.Map<Venta>(venta));
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de venta modificado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, errors = "Ha ocurrido un error al crear el registro." });
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        [HttpPost]
        public async Task<IActionResult> ModificarCompra(CompraDTO compra)
        {
            string errors = "";
            if (!Validar(compra, ref errors))
            {

                return Json(new { success = false, message = errors });
            }

            if (compra.IdTipoProducto.Equals(EProductType.Animales)) { compra.Cantidad = 1; }
            var respuesta = await _compra.UpdateAsync(_Mapper.Map<Compra>(compra));
            if (respuesta != null)
            {

                return Json(new { success = true, message = "Registro de compra modificado exitosamente." });
            }
            else
            {
                return Json(new { success = false, errors = "Ha ocurrido un error al modificar el registro." });
            }

        }


        public bool Validar(CompraDTO compra, ref string errors)
        {

            if (compra.IdTipoProducto == null) { errors = "Debe seleccionar el tipo de producto."; return false; }
            else
            {
                if (compra.IdTipoProducto.Value.Equals(EProductType.Animales))
                {
                    if (compra.IdAnimal == null) { errors = "Debe seleccionar el animal."; return false; }
                }
                else
                {
                    if (compra.IdProducto == null) { errors = "Debe seleccionar el producto."; return false; }
                    if (compra.Cantidad == null) { errors = "Debe digitar la cantidad."; return false; }
                    if (compra.Cantidad <= 0) { errors = "La cantidad debe ser mayor a 0."; return false; }
                }
            }

            if (compra.IdProveedor == null) { errors = "Debe seleccionar el proveedor."; return false; }
            if(compra.Descripcion==null) { errors = "Debe digitar una descripción."; return false; }
            if (compra.Importe == null) { errors = "Debe digitar el importe total de la compra."; return false; }
            if (compra.Importe <=0) { errors = "El importe debe ser mayor a 0."; return false; }
            if ( compra.FechaCompra == null) { errors = "Debe digitar la fecha de la compra."; return false; }



            return true;
        }
        public bool ValidarVenta(VentaDTO venta, ref string errors)
        {

            if (venta.CedulaCliente == null) { errors = "Debe digitar la cédula del cliente."; return false; }
          

            if (venta.NombreCliente == null) { errors = "Debe digitar el nombre del cliente."; return false; }
            if (venta.IdAnimal == null) { errors = "Debe seleccionar como mínimo un animal."; return false; }
            if (venta.Descripcion == null) { errors = "Debe digitar la descripción de la compra."; return false; }
            if (venta.Importe == null) { errors = "Debe digitar el importe total de la venta."; return false; }
            if (venta.Importe <=0) { errors = "El importe debe ser mayor a 0."; return false; }
            if (venta.FechaVenta == null) { errors = "Debe digitar la fecha de la venta."; return false; }


            return true;
        }
        public async Task<IActionResult> ValidarEliminacionCompra(int Id)
        {
            try
            {

                var compra = await _compra.GetByIdAsync(Id);
                TimeSpan DiasTranscurridos = DateTime.Now - compra.FechaCreacion;

                if (DiasTranscurridos.Days > 0)
                {

                    return Json(new { success = false, message = "No es posible eliminar este registro debido a que ya ha transcurrido más de 1 días desde su creación." });
                }
                else
                {
                    await _compra.DeleteAsync(Id);

                    return Json(new { success = true, message = "Registro eliminado correctamente", Data = compra });
                }
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " });
            }
        }
        public async Task<IActionResult> ValidarEliminacionVenta(int Id)
        {
            try
            {
                
                var venta = await _venta.GetByIdAsync(Id);
                TimeSpan DiasTranscurridos = DateTime.Now - venta.FechaCreacion;

                if (DiasTranscurridos.Days > 0)
                {

                    return Json(new { success = false, message = "No es posible eliminar este registro debido a que ya ha transcurrido más de 1 días desde su creación." });
                }
                else
                {
                    await _venta.DeleteAsync(Id);

                    return Json(new { success = true, message = "Registro eliminado correctamente", Data = venta });
                }
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " });
            }
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
                        var animal = await _animalModel.GetAnimalById(animalId);
                        {
                            animales.Add(animal);
                        }
                    }
                    else
                    {
                        return BadRequest($"El ID '{id}' no es válido.");
                    }
                }
                var AnimalDTO = _Mapper.Map<List<AnimalDTO>>(animales);

                return PartialView("_PartialViewAnimalesReporte", AnimalDTO);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
