using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class AnimalModel : IAnimalModel
    {

        private readonly MancariBlueContext _context;
        public AnimalModel(MancariBlueContext context)
        {

            _context = context;
        }

        public async Task<Animal> DeleteAnimal(long id)
        {
            var respueta = await _context.Animal.FindAsync(id);
            if (respueta != null)
            {
                if (respueta.Estado)
                {
                    respueta.Estado = false;

                }
                else
                {
                    respueta.Estado = true;
                }
                await _context.SaveChangesAsync();
                return respueta;
            }
            return null;
        }

        public async Task<Animal> GetAnimalById(long id)
        {
            return await _context.Animal.FindAsync(id);
        }

        public async Task<List<Animal>> GetAnimal()
        {
            return await _context.Animal.ToListAsync();
        }

        public async Task<Animal> PostAnimal(Animal animal)
        {
            await _context.Animal.AddAsync(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<Animal> CambiarEstado(long id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal != null)
            {
                if (animal.Estado)
                {
                    animal.Estado = false;

                }
                else
                {
                    animal.Estado = true;
                }
                await _context.SaveChangesAsync();
                return animal;
            }
            return null;
        }

        public async Task<Animal> PutAnimal(Animal animal)
        {
            _context.Animal.Update(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        public async Task<List<Animal>> GetAnimalStatus(bool Status)

        {
            return await _context.Animal.Where(m => m.Id != 1 && m.Estado == Status).ToListAsync();
        }
    }
}
