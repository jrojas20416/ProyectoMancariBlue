using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class CantonModel : ICantonModel
    {
        private readonly MancariBlueContext _context;

        public CantonModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Canton>> GetAllAsync()
        {
            return await _context.Cantones.ToListAsync();
        }

        public async Task<Canton> GetByIdAsync(int id)
        {
            return await _context.Cantones.FindAsync(id);
        }

        public async Task<IEnumerable<Canton>> GetByProvinciaIdAsync(int provinciaId)
        {
            return await _context.Cantones
                .Where(c => c.ProvinciaID == provinciaId)
                .ToListAsync();
        }
    }
}
