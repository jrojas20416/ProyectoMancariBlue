using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProyectoMancariBlue.Controllers
{
    public class AnimalController : Controller
    {

        readonly IAnimalModel _animalModel;
        private readonly IMapper _mapper;
        public AnimalController(IAnimalModel animalModel, IMapper mapper)
        {
            _animalModel = animalModel;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(AnimalDTO animal, [FromServices] IWebHostEnvironment hostingEnvironment)
        {

            {
                var animalE= _mapper.Map<Animal>(animal);
                var respuesta = await _animalModel.PostAnimal(animalE);
                if (respuesta != null)
                {
                  
                    return Json("Se creado el registro: "+respuesta.Codigo+" con éxito");
                }
              
                return Json("Error al crear el animal");

            }

        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

       
    }


}
