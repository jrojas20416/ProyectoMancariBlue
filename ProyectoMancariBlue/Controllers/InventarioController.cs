using Microsoft.AspNetCore.Mvc;

namespace ProyectoMancariBlue.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearProducto()
        {
            return View();
        }

        public IActionResult EditarProducto()
        {
            return View();
        }

        public IActionResult VerProducto()
        {
            return View();
        }
}
}