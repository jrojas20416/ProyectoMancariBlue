using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using System;
using System.Text;

namespace ProyectoMancariBlue.Controllers
{
    public class VacacionController : Controller
    {
        private readonly IVacacionModel _vacacionModel;
        private readonly IMapper _mapper;
        private readonly IEmpleadoModel _empleadoModel;
        public VacacionController(IVacacionModel vacacionModel, IMapper mapper, IEmpleadoModel empleadoModel)
        {
            _vacacionModel = vacacionModel;
            _mapper = mapper;
            _empleadoModel = empleadoModel;
        }
        public async Task<IActionResult> Index()
        {
            VacacionRequest request = new VacacionRequest();
            request.EmpleadoLista = _mapper.Map<IEnumerable<EmpleadoDTO>>(_empleadoModel.GetAllAsync().Result.Where(x => x.Estado).ToList());
            request.VacacionLista = _mapper.Map<IEnumerable<VacacionDTO>>(await _vacacionModel.GetAllAsync());

            return View(request);
        }


        public async Task<IActionResult> CrearVacacion(VacacionDTO vacacion)
        {
            try
            {
                string errors = "";

                if (!Validar(vacacion, ref errors))
                {

                    return Json(new { success = false, message = errors });
                }
                var empleadoE = await _empleadoModel.GetByIdAsync(vacacion.IdEmpleado.Value);

                TimeSpan DiasSolicitados = vacacion.FechaFinal.Value - vacacion.FechaInicio.Value;
                if (empleadoE.DiasDisponibles < DiasSolicitados.Days)
                {
                    return Json(new { success = false, message = "El empleado no dispone de la cantidad disponible de días para realizar la gestión" });
                }

                if (vacacion.Id == null)
                {
                    vacacion.FechaCreacion = DateTime.Now;
                }




                await _vacacionModel.AddAsync(_mapper.Map<Vacacion>(vacacion));
                empleadoE.DiasDisponibles -= DiasSolicitados.Days;
                await _empleadoModel.UpdateAsync(_mapper.Map<Empleado>(empleadoE));
                return Json(new { success = true, message = "Registro de vacaciones creado exitosamente." });
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

        public IActionResult EditarVacacion()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var vacacion = await _vacacionModel.GetByIdAsync(Id);
                var empleado = await _empleadoModel.GetByIdAsync(vacacion.IdEmpleado);
                empleado.DiasDisponibles += vacacion.DiasSolicitados;
                await _empleadoModel.UpdateEmpleado(empleado);
                await _vacacionModel.DeleteAsync(vacacion.Id);
                return Json(new { success = true, message = "Registro eliminado con éxito" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " });
            }

        }

        public IActionResult VerVacacion()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleado(long Id, DateTime StartDate, DateTime EndDate)
        {

            try
            {
                var EmpleadoDTO = _mapper.Map<EmpleadoDTO>(await _empleadoModel.GetByIdAsync(Id));


                return Json(new { success = true, message = "Registro de empleado modificado exitosamente.", data = ToString(EmpleadoDTO, StartDate, EndDate) });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string ToString(EmpleadoDTO empleadoDTO, DateTime StartDate, DateTime EndDate)
        {
            TimeSpan Days = EndDate - StartDate;
            StringBuilder builder = new StringBuilder();
            builder.Append("Colaborador: " + empleadoDTO.Nombre + "\n" + "Días diponibles: " + empleadoDTO.DiasDisponibles)
                .Append('\n')
                .Append("Fecha de inicio del permiso: " + StartDate.ToString("dd/MM/yyyy") + "\n" + "Fecha de finalización del permiso: " + EndDate.ToString("dd/MM/yyyy"))
                .Append('\n')
                .Append("Cantidad de días solicitados: " + Days.Days)
                .Append('\n');
            if (empleadoDTO.DiasDisponibles < Days.Days)
                builder.Append("Este empleado no dispone de la cantidad de días disponibles suficientes para realizar la solicitud de vacaciones entre las fechas estimadas");
            else
                builder.Append("El empleado dispone de la cantidad de días necesarios para realizar la solicitud");

            return builder.ToString();


        }

        public bool Validar(VacacionDTO vacacion, ref string errors)
        {

            if (vacacion.FechaInicio == null) { errors = "Debe seleccionar la fecha inicial del permiso"; return false; }
            if (vacacion.FechaFinal == null) { errors = "Debe seleccionar la fecha final del permiso"; return false; }
            if (vacacion.IdEmpleado == null) { errors = "Debe seleccionar el empleado"; return false; }
            return true;
        }
        public async Task<IActionResult> ValidarEliminacion(int Id)
        {
            try
            {

                var vacacion = await _vacacionModel.GetByIdAsync(Id);
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
    }
}
