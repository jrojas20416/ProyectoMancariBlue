using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoMancariBlue.Models.Clases;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Controllers
{
    public class AnimalController : Controller
    {

        readonly IAnimalModel _animalModel;

        public AnimalController(IAnimalModel animalModel)
        {
            _animalModel = animalModel;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var animal = await _animalModel.GetAnimal();
                return View(animal);
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

            public async Task<IActionResult> Create(Animal animal, [FromServices] IWebHostEnvironment hostingEnvironment)
            {

                {
                    var respuesta = await _animalModel.PostAnimal(animal);
                    if (respuesta != null)
                    {
                        TempData["AlertMessage"] = "Se creo el animal correctamente";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["AlertMessage"] = "Error al crear el animal";
                    TempData["AlertType"] = "error";
                    return View(animal);

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
            public async Task<IActionResult> Edit(Animal animal)
            {

                var respuesta = await _animalModel.PutAnimal(animal);
                if (respuesta != null)
                {

                    TempData["AlertMessage"] = "Se edito el animal correctamente";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Index));
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



        
    }
}