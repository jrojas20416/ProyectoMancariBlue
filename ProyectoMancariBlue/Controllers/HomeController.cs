using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using ProyectoMancariBlue.Models;
using ProyectoMancariBlue.Models.Obj;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using ProyectoMancariBlue.Models.Interfaces;

namespace ProyectoMancariBlue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MancariBlueContext _context;
        private readonly IEmpleadoModel _empleado;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, MancariBlueContext context, IEmpleadoModel empleado, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _empleado = empleado;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            Home home = new Home();
            home.Empleados = _context.Empleado.Count(c => c.IdRol == 1);
            home.Animales = _context.Animal.Count();

            return View(home);
        }
        [Authorize(Roles = "Empl")]
        public async Task<IActionResult> IndexC()
        {
            int id = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
            var respuesta = await _empleado.GetEmpleadoById(id);
            return View(respuesta);
        }



        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
