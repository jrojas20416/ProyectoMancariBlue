using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class ProvinciaModel : IProvinciaModel
    {
        private readonly MancariBlueContext _context;

        public ProvinciaModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Provincia>> GetAllAsync()
        {
            return await _context.Provincias.ToListAsync();
        }

        public async Task<Provincia> GetByIdAsync(int id)
        {
            return await _context.Provincias.FindAsync(id);
        }
    }
}
