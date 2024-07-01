using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedor _proveedorRepository;
        private readonly ICategoriaProveedor _categoriaProveedorRepository;
        private readonly IMapper _mapper;



        public ProveedorController(IProveedor proveedor, ICategoriaProveedor categiaProveedor, IMapper mapper)
        {

            this._proveedorRepository = proveedor;
            this._mapper = mapper;
            this._categoriaProveedorRepository = categiaProveedor;

        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var proveedor = _proveedorRepository.GetAll();
                var Categoria = _categoriaProveedorRepository.GetAll();
                if (proveedor == null)
                {
                    return NotFound();
                }

                FillDropDownListSeachCategorySupplier();
                var proveedorDTO = _mapper.Map<List<ProveedorDTO>>(proveedor);

                return View(proveedorDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desplegar los animales: {ex}");
                return View();
            }

        }

        public IActionResult CrearProveedor()
        {
            return View();
        }

        public IActionResult EditarProveedor()
        {
            return View();
        }

        public IActionResult VerProveedor()
        {
            return View();
        }

        public void FillDropDownListSeachCategorySupplier()
        {
            var CategoriaProveedor = _categoriaProveedorRepository.GetAll();

            var CategoriaProveedorList = CategoriaProveedor.Select(categoryL => new SelectListItem
            {
                Value = categoryL.Id.ToString(),
                Text = categoryL.Descripcion,

            });


            ViewBag.CategoriaProveedorListR = CategoriaProveedorList;

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(ProveedorDTO proveedor, [FromServices] IWebHostEnvironment hostingEnvironment)
        {


            var proveedorE = _mapper.Map<Proveedor>(proveedor);
            var respuesta = await _proveedorRepository.PostProveedor(proveedorE);
            if (respuesta != null)
            {

                return Json("Se creado el registro: " + respuesta.Nombre + " con éxito");
            }
            TempData["AlertMessage"] = "Error al crear el proveedor";
            TempData["AlertType"] = "error";
            return Json("Error al crear el proveedor");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(ProveedorDTO proveedor, [FromServices] IWebHostEnvironment hostingEnvironment)
        {


            var proveedorE = _mapper.Map<Proveedor>(proveedor);
            var respuesta = await _proveedorRepository.UpdateAsync(proveedorE);
            if (respuesta != null)
            {

                return Json("Se creado el registro: " + respuesta.Nombre + " con éxito");
            }
            TempData["AlertMessage"] = "Error al crear el animal";
            TempData["AlertType"] = "error";
            return Json("Error al crear el animal");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> EnableAndDisable(ProveedorDTO proveedor, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var respuesta = await _proveedorRepository.GetByIdAsync(proveedor.Id);
            if (respuesta != null)
            {
                if (respuesta.Estado)
                    respuesta.Estado = false;
                else
                    respuesta.Estado = true;
                var updatedSupplier = await _proveedorRepository.UpdateAsync(respuesta);
                if (updatedSupplier != null)
                {
                    if(!updatedSupplier.Estado)
                        return Json("Se ha deshabilitado el registro: " + respuesta.Nombre + " con éxito");
                    else
                        return Json("Se ha habilitado el registro: " + respuesta.Nombre + " con éxito");

                }

            }
            else
            {
                return Json("Error al modificar el registro: " + respuesta.Nombre);
            }

            if (respuesta != null)
            {


                return Json("Se creado el registro: " + respuesta.Nombre + " con éxito");
            }
            TempData["AlertMessage"] = "Error al crear el animal";
            TempData["AlertType"] = "error";
            return Json("Error al crear el animal");
        }

    }
}
