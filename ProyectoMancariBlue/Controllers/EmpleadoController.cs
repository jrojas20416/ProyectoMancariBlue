using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models;
using ProyectoMancariBlue.Models.Clases;
using AutoMapper;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Digests;
using System.Globalization;
using iText.Layout.Borders;
using ProyectoMancariBlue.Models.Enum;
using System.Text;
using Newtonsoft.Json;


namespace ProyectoMancariBlue.Controllers
{
    public class EmpleadoController : Controller
    {

        private readonly IEmpleadoModel _empleadoModel;
        private readonly IRolModel _rolModel;
        private readonly IMapper _mapper;
        private readonly IProvinciaModel _provinciaModel;
        private readonly ICantonModel _cantonModel;
        private readonly IDistritoModel _distritoModel;
        private readonly IHistoricoPagoModel _historicoPagoModel;
        private readonly ILiquidacionModel _liquidacionModel;
        private string layoutName = "_Layout";

        public EmpleadoRequest empleadoRequest { get; set; }

        public EmpleadoController(IEmpleadoModel empleadoModel, IRolModel rolModel, IMapper mapper, IProvinciaModel
            provinciaModel, ICantonModel cantonModel, IDistritoModel distritoModel, IHistoricoPagoModel historicoPagoModel, ILiquidacionModel liquidacionModel)
        {

            _empleadoModel = empleadoModel;
            _rolModel = rolModel;
            _mapper = mapper;
            empleadoRequest = new EmpleadoRequest();
            _provinciaModel = provinciaModel;
            _cantonModel = cantonModel;
            _distritoModel = distritoModel;
            _historicoPagoModel = historicoPagoModel;
            _liquidacionModel = liquidacionModel;
            _liquidacionModel = liquidacionModel;

        }

        public static class GlobalVariables
        {
            //  public static string MyGlobalVariable { get; set; } = layoutName;
        }

        public IActionResult AccessDenied()
        {
            TempData["AlertMessage"] = "No tiene permisos para ingresar a la pagína";
            TempData["AlertType"] = "error";
            return Redirect("Index");
        }
        [HttpGet]
        public IActionResult Index(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("Id");
                Response.Cookies.Delete("Rol");
                Response.Cookies.Delete("NombreCompleto");
                TempData["AlertMessage"] = "Se cerró correctamente la sesión";
                TempData["AlertType"] = "success";
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Index", "Empleado");
        }

        [HttpPost]
        public async Task<IActionResult> Login(EmpleadoInicio empleado, string? ReturnUrl)
        {
            var contrasena = _empleadoModel.HashPassword(empleado.EmpleadoLogin.Contrasena);
            empleado.EmpleadoLogin.Contrasena = contrasena;
            var respuesta = _empleadoModel.LoginAsync(empleado.EmpleadoLogin);

            if (respuesta.Result != null)
            {

                if (respuesta.Result.Estado)
                {
                    if (respuesta.Result.UsuarioSistema)
                    {
                        if (!respuesta.Result.Locked)
                        {
                            respuesta.Result.Locked = false;
                            respuesta.Result.LoginAttempts = 0;


                           await  _empleadoModel.UpdateAsync(respuesta.Result);

                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddHours(1),
                                Secure = true,
                                HttpOnly = true
                            };
                            Response.Cookies.Append("Id", respuesta.Result.Id.ToString(), cookieOptions);
                            Response.Cookies.Append("Rol", respuesta.Result.Rol.Nombre, cookieOptions);

                            if (respuesta.Result.RContrasena)
                            {
                                TempData["AlertMessage"] = "Se requiere Cambiar la contraseña";
                                TempData["AlertType"] = "info";
                                return RedirectToAction("CambiarContrasena");
                            }
                            else
                            {
                                Response.Cookies.Append("NombreCompleto", respuesta.Result.Nombre, cookieOptions);
                                Response.Cookies.Append("RolDescription", respuesta.Result.Rol.Descripcion, cookieOptions);

                                List<Claim> claims = new List<Claim>()
{
    new Claim(ClaimTypes.NameIdentifier, respuesta.Result.Cedula),
    new Claim("id", respuesta.Result.Id.ToString()),
    new Claim(ClaimTypes.Role, respuesta.Result.Rol.Nombre)
};

                                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties properties = new AuthenticationProperties()
                                {
                                    AllowRefresh = true
                                };

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(claimsIdentity), properties);

                                TempData["AlertMessage"] = "Inicio de Sesión";
                                TempData["AlertType"] = "success";

                                string layoutName = "_Layout"; // Valor por defecto

                                switch (respuesta.Result.Rol.Id)
                                {
                                    case 3:
                                        layoutName = "_EmpleadoLayout";
                                        break;
                                    case 4:
                                        layoutName = "_VeterinariaLayout";
                                        break;
                                    case 5:
                                        layoutName = "_BodegueroLayout";
                                        break;
                                    case 6:
                                        layoutName = "_ContadorLayout";
                                        break;
                                }

                                Response.Cookies.Append("Layout", layoutName, cookieOptions);

                                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }

                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Error, el usuario se encuentra bloqueado.";
                            TempData["AlertType"] = "error";
                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        TempData["AlertMessage"] = "Error, las credenciales ingresadas no pertenecen a un usuario del sistema.";
                        TempData["AlertType"] = "error";
                        return RedirectToAction("Index");

                    }


                }
                else
                {
                    TempData["AlertMessage"] = "Error, el usuario se encuentra inactivo";
                    TempData["AlertType"] = "error";
                    return RedirectToAction("Index");

                }

            }
            var userFound = await _empleadoModel.GetByCedulaOrEmail(empleado.EmpleadoLogin.Empleado);

            if (userFound != null && userFound.Locked)
            {
                TempData["AlertMessage"] = "Error, el usuario se encuentra bloqueado.";
                TempData["AlertType"] = "error";
                return RedirectToAction("Index");
            }
            else {
                await _empleadoModel.updateLoginattemptsUser(empleado.EmpleadoLogin.Empleado);
                TempData["AlertMessage"] = "Error, Verifique las credenciales";
                TempData["AlertType"] = "error";
                return RedirectToAction("Index");
            }

          
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerContrasena(EmpleadoInicio empleado)
        {
            var respuesta =  _empleadoModel.RContrasenaAsync(empleado.RestorePassword);

            if (respuesta != null)
            {

                TempData["AlertMessage"] = "Se restablecio su contraseña, verifique el corrreo";
                TempData["AlertType"] = "success";
                return RedirectToAction("Index");
            }
            TempData["AlertMessage"] = "Error, verifique los datos";
            TempData["AlertType"] = "error";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasena user)
        {

            var respuesta = _empleadoModel.CambiarContrasena(user);
            if (respuesta.Result)
            {
                TempData["AlertMessage"] = "Se cambio la contraseña ";
                TempData["AlertType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "Error Verifique los datos";
                TempData["AlertType"] = "error";
                return View("CambiarContrasena");
            }
        }
        [Authorize(Roles = "Admin,Dependiente")]
        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            var empleadoNormal = await _empleadoModel.GetAllAsync();
            var empleadosDTO = _mapper.Map<List<EmpleadoDTO>>(await _empleadoModel.GetAllAsync());
            empleadoRequest.Empleado = empleadosDTO;
            empleadoRequest.ListaProvincia = _mapper.Map<IEnumerable<ProvinciaDTO>>(await _provinciaModel.GetAllAsync());
            empleadoRequest.ListaRol = await _rolModel.GetRolAsync();
            // empleadoRequest.ListaCanton = _mapper.Map<IEnumerable<CantonDTO>>(await _cantonModel.GetAllAsync());
            // empleadoRequest.ListaDistrito = _mapper.Map<IEnumerable<DistritoDTO>>(await _distritoModel.GetAllAsync());

            return View(empleadoRequest);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCantones(int provinciaId)
        {
            return Json(_mapper.Map<List<CantonDTO>>(await _cantonModel.GetByProvinciaIdAsync(provinciaId)));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateDistrito(int cantonId)
        {
            return Json(_mapper.Map<List<DistritoDTO>>(await _distritoModel.GetByCantonIdAsync(cantonId)));

        }
        [Authorize(Roles = "Admin,Dependiente")]
        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var usuarios = _empleadoModel.GetEmpleados().Result.Where(x => x.UsuarioSistema && x.Estado).ToList();
            var Rol = await _rolModel.GetRolAsync();
            var EmpleadoRequest = new EmpleadoRequest()
            {
                Empleado = _mapper.Map<List<EmpleadoDTO>>(usuarios),
                ListaRol = _mapper.Map<List<Rol>>(Rol)
            };

            return View("IndexA", EmpleadoRequest);
        }
        [Authorize(Roles = "Admin,Dependiente")]
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(long id)
        {
            var respuesta = await _empleadoModel.DeleteEmpleadoID(id);
            if (respuesta != null)
            {
                return Ok();
            }
            return BadRequest();
        }


        [Authorize(Roles = "Admin,Dependiente")]
        [HttpGet]
        public async Task<IActionResult> EditarAdmin(long id)
        {
            var empleado = await _empleadoModel.GetEmpleadoById(id);
            List<Rol> roles = await _rolModel.GetRolAsync();
            ViewData["Roles"] = roles.Select(roles => new SelectListItem { Text = roles.Descripcion, Value = roles.Id.ToString() }).ToList();
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [Authorize(Roles = "Admin,Dependiente")]
        [HttpPost]

        public async Task<IActionResult> EditarAdmin(Empleado empleado)
        {
            var respuesta = await _empleadoModel.UpdateAdmin(empleado);
            if (respuesta != null)
            {
                TempData["AlertMessage"] = "Se edito correctamente el usuario Administrador";
                TempData["AlertType"] = "success";
                return RedirectToAction(nameof(GetAdmins));
            }
            TempData["AlertMessage"] = "Error al editar el usuario Administrador";
            TempData["AlertType"] = "errror";
            return View(empleado);
        }

        [Authorize(Roles = "Admin,Dependiente")]

        [HttpGet]
        public async Task<IActionResult> CrearEmpleado()
        {
            return View();
        }
        public async Task<IActionResult> IndexPrestacion()
        {
            PrestacionRequest request = new PrestacionRequest();
            request.ListaEmpleados = _mapper.Map<List<EmpleadoDTO>>(await _empleadoModel.GetAllAsync());

            return View(request);
        }
        [Authorize(Roles = "Admin,Dependiente")]

        [HttpPost]
        public async Task<IActionResult> CrearEmpleado(Empleado empleado, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var validacion = _empleadoModel.EmpleadoExists(empleado.Cedula, empleado.Correo);
                if (validacion.Result)
                {
                    return View(empleado);
                }
                empleado.Estado = true;
                empleado.RContrasena = true;
                empleado.Locked = false;
                empleado.LoginAttempts = 0;
                var respuesta = _empleadoModel.CreateEmpleado(empleado);
                if (respuesta != null)
                {

                    TempData["AlertMessage"] = "Se creo el empleado";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(GetEmpleados));
                }
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View(empleado);
            }
            catch
            {
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View(empleado);
            }
        }

        [Authorize(Roles = "Admin,Dependiente")]
        [HttpGet]
        public async Task<IActionResult> EditarEmpleado(long id)
        {
            var usuario = await _empleadoModel.GetEmpleadoById(id);


            return View(usuario);
        }
        [Authorize(Roles = "Admin,Dependiente")]
        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Empleado empleado)
        {
            var respuesta = await _empleadoModel.UpdateEmpleado(empleado);
            if (respuesta != null)
            {
                TempData["AlertMessage"] = "Se edito correctamente el empleado";
                TempData["AlertType"] = "success";
                return RedirectToAction(nameof(GetEmpleados));
            }
            TempData["AlertMessage"] = "Error al editar el empleado";
            TempData["AlertType"] = "error";
            return View(empleado.Id);
        }

        [Authorize(Roles = "Admin,Dependiente")]
        public async Task<IActionResult> VerEmpleado(long id)
        {
            var usuario = await _empleadoModel.GetEmpleadoById(id);
            if (usuario != null)
            {
                return View(usuario);

            }
            return RedirectToAction(nameof(GetEmpleados));
        }

        [Authorize(Roles = "Admin,Dependiente")]
        public async Task<IActionResult> DetailsAdmin(long id)
        {
            var usuario = await _empleadoModel.GetEmpleadoById(id);
            List<Rol> roles = await _rolModel.GetRolAsync();
            ViewData["Roles"] = roles.Select(roles => new SelectListItem { Text = roles.Descripcion, Value = roles.Id.ToString() }).ToList();
            if (usuario != null)
            {
                return View("VerAdministrador", usuario);

            }
            return RedirectToAction(nameof(GetAdmins));
        }
        [HttpPost]
        public async Task<IActionResult> CrearEmpleadoModal(EmpleadoDTO empleado)
        {
            try
            {


                string errors = "";
                if (!Validar(empleado, ref errors, false))
                {
                    return Json(new { success = false, errors });
                }
                var empleadoE = _mapper.Map<Empleado>(empleado);
                var validacion = _empleadoModel.EmpleadoExists(empleado.Cedula, empleado.Correo);
                if (validacion.Result)
                {
                    errors = "Ya existe un usuario registrado con la identificación: " + empleado.Cedula + " o con el correo: " + empleado.Correo;
                    return Json(new { success = false, errors });
                }
                //rol 2 igual a colaborador
                empleadoE.IdRol = 2;
                empleadoE.Estado = true;
                empleadoE.RContrasena = true;

                var respuesta = await _empleadoModel.CreateEmpleado(empleadoE);
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de empleado creado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, errors = "Ha ocurrido un error al crear el registro." });
                }

            }
            catch
            {
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View(empleado);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ModificarEmpleadoModal(EmpleadoDTO empleado)
        {
            try
            {
                string errors = "";
                if (!Validar(empleado, ref errors, false))
                {
                    return Json(new { success = false, errors });
                }
                var empleadoE = _mapper.Map<Empleado>(empleado);
                var validacion = _empleadoModel.EmpleadoExists(empleado.Cedula, empleado.Correo);

                var respuesta = await _empleadoModel.UpdateAsync(empleadoE);
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de empleado modificado exitosamente." });
                }
                else
                {
                    return Json(new { success = true, message = "Ha ocurrido un error al crear el registro." });
                }

            }
            catch
            {
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View(empleado);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ModificarEmpleadoModalRol(EmpleadoDTO empleado)
        {
            try
            {
                var empleadoFound = await _empleadoModel.GetByIdAsync(empleado.Id.Value);
                string errors = "";
                if (!ValidarRol(empleadoFound, ref errors, empleado.IdRol.Value))
                {
                    return Json(new { success = false, errors });
                }

                var respuesta = await _empleadoModel.UpdateAsync(empleadoFound);
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de usuario modificado exitosamente." });
                }
                else
                {
                    return Json(new { success = true, message = "Ha ocurrido un error al crear el registro." });
                }

            }
            catch
            {
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View("");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GenerarContrasenna(EmpleadoDTO empleado)
        {
            try
            {
                var empleadoFound = await _empleadoModel.GetByIdAsync(empleado.Id.Value);


                var respuesta = await _empleadoModel.UpdateAsyncRole(empleadoFound);
                if (respuesta != null)
                {

                    return Json(new { success = true, message = "Registro de usuario modificado exitosamente." });
                }
                else
                {
                    return Json(new { success = true, message = "Ha ocurrido un error al crear el registro." });
                }

            }
            catch
            {
                TempData["AlertMessage"] = "Verique los datos, error al crear el empleado";
                TempData["AlertType"] = "error";
                return View("");
            }
        }

        public bool Validar(EmpleadoDTO empleado, ref string errors, bool IsModify)
        {
            if (empleado.Cedula == null)
            {
                errors = "Debe digitar la identificación";
                return false;
            }
            if (empleado.Nombre == null)
            {
                errors = "Debe escribir el nombre";
                return false;
            }
            if (empleado.Nacionalidad == null)
            {
                errors = "Debe escribir la nacionalidad";
                return false;
            }
            if (empleado.Correo == null)
            {
                errors = "Debe escribir el correo";
                return false;
            }
            else
            {
                if (!IsValidEmail(empleado.Correo))
                {
                    errors = "El formato del correo no es válido";
                    return false;
                }
            }
            if (empleado.Edad == null)
            {
                errors = "Debe digitar la edad";
                return false;
            }
            if (empleado.Edad <18)
            {
                errors = "La edad debe ser mayor o igual a 18.";
                return false;
            }
            if (empleado.Salario == null)
            {
                errors = "Debe digitar el salario";
                return false;
            }
            if (empleado.Salario <=0)
            {
                errors = "El salario debe ser mayor a 0.";
                return false;
            }
            if (empleado.ProvinciaId == null)
            {
                errors = "Debe seleccionar la provincia";
                return false;
            }

            if (empleado.CantonId == null)
            {
                errors = "Debe seleccionar el cantón";
                return false;
            }

            if (empleado.DistritoId == null)
            {
                errors = "Debe seleccionar el distrito";
                return false;
            }
            if (empleado.Puesto == null)
            {
                errors = "Debe digitar el puesto";
                return false;
            }
            if (empleado.FechaIngreso == null)
            {
                errors = "Debe digitar la fecha de ingreso";
                return false;
            }
            if (empleado.Telefono == null)
            {
                errors = "Debe digitar el número de teléfono";
                return false;
            }
            //if (empleado.IdRol == null)
            //{
            //    errors = "Debe seleccionar el rol";
            //    return false;
            //}
            return true;
        }
        public bool ValidarRol(Empleado empleado, ref string errors, long nuevorol)
        {

            if (empleado.IdRol == nuevorol)
            {
                errors = "El rol debe ser distinto al actual.";
                return false;
            }
            else
            {
                empleado.IdRol = nuevorol;
            }
            return true;
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpleadoById(int id)
        {
            var empleado = _mapper.Map<EmpleadoDTO>(await _empleadoModel.GetByIdAsync(id));
            if (empleado != null)
            {

                return Json(new { success = true, data = empleado });
            }
            return Json(new { success = false, message = "Empleado no encontrado" });
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerPagos(long id)
        {
            try
            {

                var PagosE = _historicoPagoModel.GetPagosByEmpleadoIdAsync(id).Result.Take(3).ToList();
                var Pagos = _mapper.Map<IEnumerable<HistoricoPagoDTO>>(PagosE);
                if (Pagos != null)
                {
                    return PartialView("_PartialViewGenerarPlanilla", Pagos);

                }
                return Json(new { success = false, message = "Error al buscar los pagos del empleado." });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " + ex.Message });
            }

        }
        [HttpGet]
        public async Task<IActionResult> ActualizarEmpleado(long id)
        {
            try
            {
                var Empleado = _empleadoModel.GetByIdAsync(id).Result;
                Empleado.Estado = false;
                await _empleadoModel.UpdateAsync(Empleado);
                return Json(new { success = false, message = "" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " + ex.Message });
            }

        }
        [HttpGet]
        public async Task<IActionResult> ObtenerPagosAguinaldo(long id)
        {
            try
            {

                var PagosE = _historicoPagoModel.GetPagosDesdeDiciembre(id).Result.Take(12).ToList();
                var Pagos = _mapper.Map<IEnumerable<HistoricoPagoDTO>>(PagosE);
                if (Pagos != null)
                {
                    return PartialView("_PartialViewGenerarAguinaldo", Pagos);

                }
                return Json(new { success = false, message = "Error al buscar los pagos del empleado." });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " + ex.Message });
            }

        }
        [HttpGet]
        public async Task<IActionResult> ObtenerPagosAguinaldoTotal(long id)
        {
            try
            {
                var PagosE = _historicoPagoModel.GetPagosDesdeDiciembre(id).Result.Take(12).ToList();
                var Pagos = _mapper.Map<IEnumerable<HistoricoPagoDTO>>(PagosE);
                if (Pagos != null)
                {
                    return PartialView("_PartialViewGenerarAguinaldoTotal", Pagos);

                }
                return Json(new { success = false, message = "Error al buscar los pagos del empleado." });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Ha ocurrido un error: " + ex.Message });
            }

        }
        public async Task<IActionResult> GenerarPDF(long empleadoId)
        {

            var empleado = await _empleadoModel.GetEmpleadoById(empleadoId);
            var pagos = _historicoPagoModel.GetPagosByEmpleadoIdAsync(empleadoId).Result.OrderByDescending(x => x.FechaPago).Take(3).ToList();

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);


                document.Add(new Paragraph("Planilla del empleado")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                // Datos del empleado
                document.Add(new Paragraph($"Nombre: {empleado.Nombre}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Puesto: {empleado.Puesto}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Fecha Ingreso: {empleado.FechaIngreso:yyyy-MM-dd}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Antigüedad (Años): {(DateTime.Now.Year - empleado.FechaIngreso.Year)}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Salario: {empleado.Salario.ToString("C", new CultureInfo("es-CR"))}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Moneda: Colones")
                    .SetFontSize(12));


                document.Add(new Paragraph(" "));


                float[] columnWidths = { 1, 1, 1, 1, 1, 1, 1 };
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100));


                Style headerStyle = new Style()
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Patrono")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Salario Bruto")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("CCSS (9.67%)")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("LPT (1%)")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total Deducciones")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Salario Neto")).AddStyle(headerStyle));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha de pago")).AddStyle(headerStyle));


                Style dataStyle = new Style()
                    .SetTextAlignment(TextAlignment.CENTER);


                foreach (var pago in pagos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(pago.Patrono)).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.SalarioBruto.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.CCSS.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.LPT.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.TotalDeducciones.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.SalarioNeto.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table.AddCell(new Cell().Add(new Paragraph(pago.FechaPago?.ToString("yyyy-MM-dd"))).AddStyle(dataStyle));
                }


                document.Add(table);

                document.Close();
                var byteInfo = ms.ToArray();

                return File(byteInfo, "application/pdf", $"{empleado.Nombre}_Planilla.pdf");
            }
        }

        public async Task<IActionResult> GenerarPDFAguinaldo(long empleadoId)
        {
            var empleado = await _empleadoModel.GetEmpleadoById(empleadoId);
            var pagos = await _historicoPagoModel.GetPagosByEmpleadoIdAsync(empleadoId);
            var pagosDesdeDiciembre = pagos
                .Where(p => p.FechaPago >= new DateTime(DateTime.Now.Year - 1, 12, 1))
                .OrderBy(p => p.FechaPago)
                .ToList();

            decimal totalSalarios = pagosDesdeDiciembre.Sum(p => p.SalarioBruto);
            decimal aguinaldo = totalSalarios / 12;

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Generar Aguinaldo")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Nombre: {empleado.Nombre}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Puesto: {empleado.Puesto}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Fecha Ingreso: {empleado.FechaIngreso:yyyy-MM-dd}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Antigüedad (Años): {(DateTime.Now.Year - empleado.FechaIngreso.Year)}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Salario: ₡ {empleado.Salario.ToString("C", new CultureInfo("es-CR"))}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Moneda: Colones")
                 .SetFontSize(12));

                document.Add(new Paragraph(" "));

                float[] columnWidths = { 1, 1 };
                iText.Layout.Element.Table table1 = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100));

                Style headerStyle = new Style()
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold();

                table1.AddHeaderCell(new Cell().Add(new Paragraph("Mes")).AddStyle(headerStyle));
                table1.AddHeaderCell(new Cell().Add(new Paragraph("Salario Bruto")).AddStyle(headerStyle));

                Style dataStyle = new Style()
                    .SetTextAlignment(TextAlignment.CENTER);

                foreach (var pago in pagosDesdeDiciembre)
                {
                    table1.AddCell(new Cell().Add(new Paragraph("₡ " + pago.FechaPago.Value.ToString("MMMM yyyy", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                    table1.AddCell(new Cell().Add(new Paragraph("₡ " + pago.SalarioBruto.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                }

                document.Add(table1);

                document.Add(new Paragraph(" "));
                iText.Layout.Element.Table summaryTable = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }))
                    .SetWidth(UnitValue.CreatePercentValue(100));
                summaryTable.AddCell(new Cell().Add(new Paragraph("Total Salarios")).AddStyle(headerStyle));
                summaryTable.AddCell(new Cell().Add(new Paragraph("Total Aguinaldo")).AddStyle(headerStyle));
                summaryTable.AddCell(new Cell().Add(new Paragraph("₡ " + totalSalarios.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));
                summaryTable.AddCell(new Cell().Add(new Paragraph("₡ " + aguinaldo.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));

                document.Add(summaryTable);

                document.Close();
                var byteInfo = ms.ToArray();

                return File(byteInfo, "application/pdf", $"{empleado.Nombre}_Aguinaldo.pdf");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GenerarPDFLiquidacion(long IdEmpleado, string FechaSalida, int? MotivoSalida, bool Preaviso, string empleadoObj)
        {
            try
            {
                string errors = "";
                if (!Validarliquidacion(IdEmpleado, FechaSalida, MotivoSalida, Preaviso, empleadoObj, ref errors))
                {
                    return Json(new { success = false, message = errors });
                }

                EmpleadoDTO empleado = _mapper.Map<EmpleadoDTO>(await _empleadoModel.GetByIdAsync(IdEmpleado));
                LiquidacionDTO liquidacion = new()
                {
                    FechaSalida = DateTime.TryParse(FechaSalida, out var fs) ? fs : (DateTime?)null,
                    IdEmpleado = IdEmpleado,
                    MotivoSalida = (EReasonDeparture)MotivoSalida,
                    Preaviso = Preaviso,
                    empleadoObj = empleado
                };
                empleado.Estado = false;

                decimal aguinaldoProporcional = LiquidacionCalculator.CalcularAguinaldoProporcional(liquidacion);
                decimal vacacionesNoDisfrutadas = LiquidacionCalculator.CalcularVacaciones(liquidacion);
                decimal preaviso = liquidacion.Preaviso ? LiquidacionCalculator.CalcularPreaviso(liquidacion) : 0;
                decimal cesantia = LiquidacionCalculator.CalcularCesantia(liquidacion);
                decimal totalLiquidacion = aguinaldoProporcional + vacacionesNoDisfrutadas + preaviso + cesantia;

                RegistroLiquidacionDTO registroL = new()
                {
                    FechaSalida = DateTime.TryParse(FechaSalida, out var fss) ? fss : (DateTime?)null,
                    IdEmpleado = IdEmpleado,
                    MotivoSalida = (EReasonDeparture)MotivoSalida,
                    Preaviso = Preaviso,
                    AguinaldoPP = aguinaldoProporcional,
                    VacacionesNoGozadas = vacacionesNoDisfrutadas,
                    PreavisoV = preaviso,
                    Cesantia = cesantia,
                    TotalLiquidacion = totalLiquidacion
                };
                await _liquidacionModel.AddAsync(_mapper.Map<RegistroLiquidacion>(registroL));

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    document.Add(new Paragraph("Liquidación del empleado")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20)
                        .SetBold());

                    document.Add(new Paragraph($"Nombre: {empleado.Nombre}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Puesto: {empleado.Puesto}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Fecha Ingreso: {empleado.FechaIngreso:yyyy-MM-dd}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Antigüedad (Años): {(DateTime.Now.Year - empleado.FechaIngreso.Value.Year)}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Salario: {empleado.Salario.Value.ToString("C", new CultureInfo("es-CR"))}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"Vacaciones disponibles: {empleado.DiasDisponibles.ToString()}")
                        .SetFontSize(12));

                    document.Add(new Paragraph(" "));

                    float[] columnWidths = { 1, 1 };
                    iText.Layout.Element.Table table1 = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnWidths))
                        .SetWidth(UnitValue.CreatePercentValue(100));

                    Style headerStyle = new Style()
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold();

                    table1.AddHeaderCell(new Cell().Add(new Paragraph("Concepto")).AddStyle(headerStyle));
                    table1.AddHeaderCell(new Cell().Add(new Paragraph("Monto")).AddStyle(headerStyle));

                    Style dataStyle = new Style()
                        .SetTextAlignment(TextAlignment.CENTER);

                    table1.AddCell(new Cell().Add(new Paragraph("Aguinaldo Proporcional")).AddStyle(dataStyle));
                    table1.AddCell(new Cell().Add(new Paragraph(aguinaldoProporcional.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));

                    table1.AddCell(new Cell().Add(new Paragraph("Preaviso")).AddStyle(dataStyle));
                    table1.AddCell(new Cell().Add(new Paragraph(preaviso.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));

                    table1.AddCell(new Cell().Add(new Paragraph("Cesantía")).AddStyle(dataStyle));
                    table1.AddCell(new Cell().Add(new Paragraph(cesantia.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));

                    table1.AddCell(new Cell().Add(new Paragraph("Vacaciones No Disfrutadas")).AddStyle(dataStyle));
                    table1.AddCell(new Cell().Add(new Paragraph(vacacionesNoDisfrutadas.ToString("C", new CultureInfo("es-CR")))).AddStyle(dataStyle));

                    table1.AddCell(new Cell().Add(new Paragraph("Total Liquidación")).AddStyle(headerStyle));
                    table1.AddCell(new Cell().Add(new Paragraph(totalLiquidacion.ToString("C", new CultureInfo("es-CR")))).AddStyle(headerStyle));

                    document.Add(table1);

                    document.Close();
                    var byteInfo = ms.ToArray();

                    var base64 = Convert.ToBase64String(byteInfo);

                    return Json(new { success = true, message = "PDF generado exitosamente.", IdE = IdEmpleado, pdfBase64 = base64, nombrePdf = empleado.Nombre + "_" + "Liquidacion.pdf" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ha ocurrido un error. " + ex.Message });
            }
        }
        [HttpGet]
        public IActionResult ObtenerDetalleLiquidacion(long IdEmpleado, string FechaSalida, int MotivoSalida, bool Preaviso, string empleadoObj)
        {

            try
            {
                EmpleadoDTO Empleado = JsonConvert.DeserializeObject<EmpleadoDTO>(empleadoObj);
                LiquidacionDTO liquidacion = new()
                {
                    FechaSalida = DateTime.TryParse(FechaSalida, out var fs) ? fs : (DateTime?)null,
                    IdEmpleado = IdEmpleado,
                    MotivoSalida = (EReasonDeparture)MotivoSalida,
                    Preaviso = Preaviso,
                    empleadoObj = Empleado

                };


                return Json(new { success = true, message = "Registro de empleado modificado exitosamente.", data = DetalleLiquidacion(liquidacion) });
            }
            catch (Exception ex)
            {

                return Json(new { success = true, message = "Ha ocurrido un error. " + ex.Message });
            }
        }
        public string DetalleLiquidacion(LiquidacionDTO liquidacion)
        {
            StringBuilder builder = new StringBuilder();
            CultureInfo culture = new CultureInfo("es-CR");

            decimal AguinaldoProporcional = LiquidacionCalculator.CalcularAguinaldoProporcional(liquidacion);
            decimal Preaviso = LiquidacionCalculator.CalcularPreaviso(liquidacion);
            decimal Cesantia = LiquidacionCalculator.CalcularCesantia(liquidacion);
            decimal Vacaciones = LiquidacionCalculator.CalcularVacaciones(liquidacion);
            builder.AppendLine("Detalles de la liquidación")

                   .AppendLine($"Aguinaldo proporcional: {AguinaldoProporcional.ToString("C", culture)}");

            if (liquidacion.Preaviso)
            {
                builder.AppendLine($"Preaviso: {Preaviso.ToString("C", culture)}");
            }
            else
            {
                builder.AppendLine("Preaviso: 0     Nota: No realizó preaviso");
            }

            builder.AppendLine($"Cesantía: {Cesantia.ToString("C", culture)}")
                   .AppendLine($"Vacaciones no gozadas: {Vacaciones.ToString("C", culture)}")
                   .AppendLine("----------------------------------------------------------------------")
                   .AppendLine("Total: " + (AguinaldoProporcional + Preaviso + Cesantia + Vacaciones).ToString("C", culture));

            return builder.ToString();
        }

        public bool Validarliquidacion(long IdEmpleado, string FechaSalida, int? MotivoSalida, bool Preaviso, string empleadoObj, ref string errors)
        {

            if (MotivoSalida == null) { errors = "Debe seleccionar el motivo de la salida"; return false; }
            if (FechaSalida == null) { errors = "Debe seleccionar la fecha de salida"; return false; }
            if (empleadoObj == null) { errors = "Ha ocurrido un error al encontrar el empleado"; return false; }
            var registro = _empleadoModel.GetEmpleadoById(IdEmpleado).Result;
            if (!registro.Estado) { errors = "No es posible generar la liquidación del empleado porque se encuentra inactivo"; return false; }
            return true;
        }
        public async Task<IActionResult> Disable(int id)
        {
            try
            {
                var usuario = await _empleadoModel.GetByIdAsync(id);
                usuario.UsuarioSistema = false;
                await _empleadoModel.UpdateAsync(usuario);
                return Json(new { success = true, message = "Usuario deshabilitado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Error al inhabilitar el usuario " + ex.Message });
            }

        }

    }
}

