using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Enum;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using ProyectoMancariBlue.Models.Obj.Request;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static ProyectoMancariBlue.Controllers.EmpleadoController;

namespace ProyectoMancariBlue.Controllers
{
    public class AnimalController : Controller
    {

        private readonly IAnimalModel _animalModel;
        private readonly IMapper _mapper;
        private readonly IRegistroVacuna _registroVacuna;
        private readonly IProducto _producto;
        public AnimalController(IAnimalModel animalModel, IMapper mapper,IRegistroVacuna registro, IProducto producto)
        {
            _animalModel = animalModel;
            _mapper = mapper;
            _registroVacuna = registro;
            _producto = producto;
        }

        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Index()
        {
            try
            {
               
                var animal = _animalModel.GetAnimal();
                if (animal == null)
                {
                    return NotFound();
                }

                var animalDto = _mapper.Map < List<AnimalDTO>>(animal);
                FillDropDownListSeachFather();
                FillDropDownListSeachMother();
                return View(animalDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desplegar los animales: {ex}");
                return View();
            }

        }
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> IndexG()
        {
            try
            {
               
                var Father = _animalModel.SearchByGender("M");
                var Mothers = _animalModel.SearchByGender("H");
                if (Father == null || Mothers==null)
                {
                    return NotFound();
                }
                AnimalRequest request = new AnimalRequest() {
                    Fathers = _mapper.Map<List<AnimalDTO>>(Father), 
                    Mothers=_mapper.Map<List<AnimalDTO>>(Mothers)
                
                };

                return View(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desplegar los animales: {ex}");
                return View();
            }

        }

        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var animal = await _animalModel.GetAnimalById(id);
                return View(animal);
            }
            catch (Exception ex)
            {
                return NotFound();

            }
        }

        [Authorize(Roles = "Admin,Veterinario")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Veterinario")]

        public async Task<IActionResult> Create(AnimalDTO animal, [FromServices] IWebHostEnvironment hostingEnvironment)
        {

            try{
                var animalE= _mapper.Map<Animal>(animal);
                var respuesta = await _animalModel.PostAnimal(animalE);
                if (respuesta != null)
                {
                  
                    return Json("Se creado el registro: "+respuesta.Codigo+" con éxito");
                }
              
                return Json("Error al crear el animal");

            }
            catch (Exception e){

                return Json("Error al crear el animal "+ e.Message);
            }

        }

        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var animal = await _animalModel.GetAnimalById(id);

                return View(animal);
            }
            catch (Exception ex)
            {
                return NotFound();

            }

        }


        [HttpPost]
        [Authorize(Roles = "Admin,Veterinario")]
        public async Task<IActionResult> Edit(AnimalDTO animal)
        {
            var animalE=_mapper.Map<Animal>(animal);
            var respuesta = await _animalModel.PutAnimal(animalE);
            if (respuesta != null)
            {

                return Json("Se modificó el registro: " + respuesta.Codigo + " con éxito");
               
            }
            TempData["AlertMessage"] = "Error al editar el animal";
            TempData["AlertType"] = "error";
            return View(animal);
        }

        [Authorize(Roles = "Admin,Veterinario")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var rspuesta = await _animalModel.DeleteAnimal(id);
            if (rspuesta != null)
            {
                return Ok();
            }
            return BadRequest();
        }

      
        public  void FillDropDownListSeachFather()
        {
            var Animal = _animalModel.SearchByGender("M");

            var AnimalList = Animal.Select(animalL => new SelectListItem
            {
                Value = animalL.Id.ToString(),
                Text = animalL.Codigo,

            });


            ViewBag.animalList = AnimalList;

        }
    
        public void FillDropDownListSeachMother()
        {
            var Animal = _animalModel.SearchByGender("H");

            var AnimalList = Animal.Select(animalL => new SelectListItem
            {
                Value = animalL.Id.ToString(),
                Text = animalL.Codigo,

            });


            ViewBag.animalMotherList = AnimalList;

        }

        [HttpGet]
        public IActionResult ObtenerVacunas(long idAnimal)
        {
            var vacunas = _registroVacuna.GetListByIdAnimal(idAnimal);
            var animales = _animalModel.GetAnimal();
            var productos = _producto.GetAllAsync().Result.Where(x => x.IdTipoProducto.Equals(EProductType.Vacunas) && x.Estado == true).ToList();

            if (vacunas == null)
            {
                return NotFound();
            }
            var VacunasDTO = _mapper.Map<List<RegistroVacunaDTO>>(vacunas);

           
            return PartialView("PartialViewRegistroVacuna", VacunasDTO);
        }
       [HttpPost]
        public async Task<IActionResult> ObtenerCriasPadre(AnimalDTO animal)
        {
            try
            {
               
                if (animal.Genero.ToUpper().Equals("M"))
                {
                    var Hijos = _animalModel.GetAnimal().Where(x => x.Padre == animal.Id);
                    if (Hijos == null)
                    {
                        return NotFound();
                    }
                    return PartialView("PartialViewHijos", _mapper.Map<List<AnimalDTO>>(Hijos));
                }
                else {

                    var Hijos = _animalModel.GetAnimal().Where(x => x.Madre == animal.Id);
                    if (Hijos == null)
                    {
                        return NotFound();
                    }
                    return PartialView("PartialViewHijos", _mapper.Map<List<AnimalDTO>>(Hijos));
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desplegar los animales: {ex}");
                return View();
            }

        }
    }


}
