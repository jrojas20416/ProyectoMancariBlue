using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class LiquidacionModel : ILiquidacionModel
    {
        private readonly MancariBlueContext _context;

        public LiquidacionModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistroLiquidacion>> GetAllAsync()
        {
            return await _context.RegistroLiquidacion.ToListAsync();
        }

        public async Task<RegistroLiquidacion> GetByIdAsync(long id)
        {
            return await _context.RegistroLiquidacion
                .Include(l => l.Empleado)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<RegistroLiquidacion> GetByIdEmpleadoAsync(long id)
        {
            return await _context.RegistroLiquidacion
                .Include(l => l.Empleado)
                .FirstOrDefaultAsync(l => l.IdEmpleado == id);
        }


        public async Task AddAsync(RegistroLiquidacion registroLiquidacion)
        {
            _context.RegistroLiquidacion.Add(registroLiquidacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RegistroLiquidacion registroLiquidacion)
        {
            _context.Entry(registroLiquidacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var registroLiquidacion = await _context.RegistroLiquidacion.FindAsync(id);
            if (registroLiquidacion != null)
            {
                _context.RegistroLiquidacion.Remove(registroLiquidacion);
                await _context.SaveChangesAsync();
            }
        }

    }
}
