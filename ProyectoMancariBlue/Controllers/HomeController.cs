using Microsoft.AspNetCore.Mvc;
using ProyectoMancariBlue.Models;
using System.Diagnostics;

namespace ProyectoMancariBlue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginAut()
        {
            return RedirectToAction("Index");
        }

        public IActionResult RestablecerContrasena()
        {
            TempData["AlertMessage"] = "Se restablecio su contraseña, verifique el corrreo";
            TempData["AlertType"] = "success";
            return RedirectToAction("Login");
        }

        public IActionResult CambiarContrasena()
        {
      
            return View();
        }

        public IActionResult CambiarContrasenaS()
        {

            TempData["AlertMessage"] = "Se cambio la contraseña correctamente";
            TempData["AlertType"] = "success";
            return RedirectToAction("Login");
        }

        public IActionResult CerrarSesion()
        {
            TempData["AlertMessage"] = "Se cerró correctamente la sesión";
            TempData["AlertType"] = "success";
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
