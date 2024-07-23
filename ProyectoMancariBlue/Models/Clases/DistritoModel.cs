using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class DistritoModel : IDistritoModel
    {
        private readonly MancariBlueContext _context;

        public DistritoModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Distrito>> GetAllAsync()
        {
            return await _context.Distritos.ToListAsync();
        }

        public async Task<Distrito> GetByIdAsync(int id)
        {
            return await _context.Distritos.FindAsync(id);
        }

        public async Task<IEnumerable<Distrito>> GetByCantonIdAsync(int cantonId)
        {
            return await _context.Distritos
                .Where(d => d.CantonID == cantonId)
                .ToListAsync();
        }
    }
}
