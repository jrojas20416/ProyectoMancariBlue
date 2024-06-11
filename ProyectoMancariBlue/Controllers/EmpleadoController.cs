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

namespace ProyectoMancariBlue.Controllers
{
    public class EmpleadoController : Controller
    {

        private readonly MancariBlueContext _context;
        private readonly IEmpleadoModel _empleadoModel;
        private readonly IRolModel _rolModel;

        public EmpleadoController(MancariBlueContext context, IEmpleadoModel empleadoModel, IRolModel rolModel)
        {
            _context = context;
            _empleadoModel = empleadoModel;
            _rolModel = rolModel;
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
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(1), // Establece la expiración de la cookie
                    Secure = true, // Establece la cookie como segura si utilizas HTTPS
                    HttpOnly = true // Evita que el token sea accesible desde JavaScript
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
                    Response.Cookies.Append("NombreCompleto", respuesta.Result.Nombre + " " + respuesta.Result.Apellidos, cookieOptions);

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

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                    TempData["AlertMessage"] = "Inicio de Sesión";
                    TempData["AlertType"] = "success";
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    if (respuesta.Result.Rol.Nombre == "Empl")
                    {

                        return RedirectToAction("IndexA", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }

            }
            TempData["AlertMessage"] = "Error, Verif|ique las credenciales";
            TempData["AlertType"] = "error";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerContrasena(EmpleadoInicio empleado)
        {
            var respuesta = _empleadoModel.RContrasenaAsync(empleado.RestorePassword);

            if (respuesta.Result != null)
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
                return View("Cambiarontrasena");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetEmpleados()

        {
            var empleados = await _empleadoModel.GetAllEmpleadosEstado(2);
            return View(empleados);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var usuarios = await _empleadoModel.GetEmpleados();
            return View("IndexA",usuarios);
        }
        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> CrearEmpleado()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]

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
                    empleado.IdRol = 2;
                empleado.Estado = true;
                empleado.RContrasena = true;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditarEmpleado(long id)
        {
            var usuario = await _empleadoModel.GetEmpleadoById(id);
          

            return View(usuario);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Empleado empleado)
        {
            var respuesta = await _empleadoModel.UpdateEmpleado(empleado); // Asegúrate de usar await aquí
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VerEmpleado(long id)
        {
            var usuario = await _empleadoModel.GetEmpleadoById(id);
            if (usuario != null)
            {
                return View(usuario);

            }
            return RedirectToAction(nameof(GetEmpleados));
        }

    }
}

