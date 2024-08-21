using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using ProyectoMancariBlue.Models;
using ProyectoMancariBlue.Models.Obj;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using ProyectoMancariBlue.Models.Interfaces;
using static ProyectoMancariBlue.Controllers.EmpleadoController;

namespace ProyectoMancariBlue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MancariBlueContext _context;
        private readonly IEmpleadoModel _empleado;
        private readonly IVenta _venta;
        private readonly ICompra _compra;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, MancariBlueContext context, IEmpleadoModel empleado, IHttpContextAccessor httpContextAccessor, IVenta venta,ICompra compra)
        {
            _logger = logger;
            _context = context;
            _empleado = empleado;
            _httpContextAccessor = httpContextAccessor;
            _venta = venta;
            _compra = compra;
        }
        
        public IActionResult Index()
        {
            Home home = new Home();
            home.Empleados = _context.Empleado.Count(c => c.Estado );
            home.Animales = _context.Animal.Count(a=>a.Estado);
            home.Inventario=_context.Producto.Count(I=>I.Stock>0 && I.Estado);
            home.Transaciones=_context.Compra.Count()+_context.Venta.Count();
           
            return View(home);
        }
        [Authorize(Roles = "Empl,Admin")]
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
