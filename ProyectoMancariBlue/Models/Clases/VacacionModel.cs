using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class VacacionModel : IVacacionModel
    {
        private readonly MancariBlueContext _context;

        public VacacionModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vacacion>> GetAllAsync()
        {
            return await _context.Vacacion.ToListAsync();
        }

        public async Task<Vacacion> GetByIdAsync(int id)
        {
            return await _context.Vacacion.FindAsync(id);
        }

        public async Task AddAsync(Vacacion vacacion)
        {
            _context.Vacacion.Add(vacacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vacacion vacacion)
        {
            _context.Vacacion.Update(vacacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vacacion = await _context.Vacacion.FindAsync(id);
            if (vacacion != null)
            {
                _context.Vacacion.Remove(vacacion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
