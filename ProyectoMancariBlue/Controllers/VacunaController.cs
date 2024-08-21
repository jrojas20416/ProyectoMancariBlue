using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Enum;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using static ProyectoMancariBlue.Controllers.EmpleadoController;

namespace ProyectoMancariBlue.Controllers
{
    public class VacunaController : Controller
    {
        private readonly IRegistroVacuna _registroVacunaModel;
        private readonly IMapper _mapper;
        private readonly IProducto _producto;
        private readonly IAnimalModel _animalModel;
        private readonly IDistritoModel _istritoModel;
       
        public VacunaController(IRegistroVacuna registroVacunaModel , IMapper mapper , IProducto producto, IAnimalModel _animal, IDistritoModel distritoModel ) {
            _registroVacunaModel = registroVacunaModel;
            _mapper = mapper;
            _producto = producto;
            _animalModel = _animal;
            _istritoModel = distritoModel;
            

        }
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Index()
        {
            var vacunas=_registroVacunaModel.GetList();
            var animales = _animalModel.GetAnimal().Where(x=>x.Estado).ToList();
            var productos =  _producto.GetAllAsync().Result.Where(x=>x.IdTipoProducto.Equals(EProductType.Vacunas) && x.Estado==true).ToList();
            var distritos = await _istritoModel.GetAllAsync();
            if (vacunas == null || animales==null || productos==null) { return NotFound(); }
            var VacunaViewDTO = new VacunaRequest()
            {
                ListaRegistroVacunaView = _mapper.Map<List<RegistroVacunaDTO>>(vacunas),
                ListaAnimalView=_mapper.Map<List<AnimalDTO>>(animales),
                ListaProductoView=_mapper.Map<List<ProductoDto>>(productos)
            };
           
            return View(VacunaViewDTO);
        }


        [HttpPost]
        public async Task<IActionResult> CrearRegistroVacuna(RegistroVacunaDTO model)
        {
            try
            {
                var errors = "";
                var producto = await _producto.GetByIdAsync(model.IdProducto.Value);
                if (Validar(model, ref errors, producto))
                {
                    var animal=await _animalModel.GetAnimalById(model.IdAnimal.Value);
                    animal.FechaUltimaVacuna = model.FechaAplicacion.Value;
                    var RegistroVacuna = _mapper.Map<RegistroVacuna>(model);
                    await _registroVacunaModel.PostRegistroVacuna(RegistroVacuna);
                    producto.Stock -= 1;
                    _producto.Update(producto);
                    await _animalModel.PutAnimal(animal);

                    return Json(new { success = true, message = "Registro de vacuna creado exitosamente." });
                }
                return Json(new { success = false, errors });
            }
            catch (Exception e)
            { 

                return Json(new { success = false, e.Message });
            }
          
        }


        public async Task<IActionResult> EditarVacuna(RegistroVacunaDTO model)
        {

            var errors = "";
            var producto = await _producto.GetByIdAsync(model.IdProducto.Value);
            if (Validar(model, ref errors, producto))
            {
                var RegistroVacuna = _mapper.Map<RegistroVacuna>(model);
                await _registroVacunaModel.PutRegistroVacuna(RegistroVacuna);
               

                return Json(new { success = true, message = "Registro de vacuna modificado exitosamente." });
            }
            return Json(new { success = false, errors });
        }
        [HttpDelete]
        public async Task< IActionResult> EliminarRegistro(int id)
        {
            try
            {
                await _registroVacunaModel.DeleteRegistroVacuna(id);

                return Json(new { success = true, message = "Registro de vacuna eliminado exitosamente." });

            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Ha ocurrido un error." });
            }
        }
        public IActionResult VerVacuna()
        {
            return View();
        }
        public bool Validar(RegistroVacunaDTO model, ref string errors,Producto producto) {
            if (model.IdAnimal == null) { 
                errors = "Debe seleccionar el animal";
            return false; }
            if (model.IdProducto == null) { 
                errors = "Debe seleccionar la vacuna a aplicar";
                return false;
            }
            if (model.FechaAplicacion == null)
            {
                errors = "Debe seleccionar la fecha a aplicar la vacuna";
                return false;
            }
            if (producto.Stock < 1) {
                errors = "El producto agotó su existencia.";
                return false;
            }

            return true;
        }
      

     

    }
}
