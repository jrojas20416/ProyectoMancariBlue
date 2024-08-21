using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class VentaModel:IVenta
    {
         private readonly MancariBlueContext _context;

        public VentaModel(MancariBlueContext context)
        {
            _context = context;
        }

        public IEnumerable<Venta> GetAll()
        {
            return _context.Venta
                
                .ToList();
        }

        public async Task<Venta> GetByIdAsync(long id)
        {
            return await _context.Venta
               
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venta> PostVenta(Venta venta)
        {
            await _context.Venta.AddAsync(venta);
            await _context.SaveChangesAsync();
            return venta;
        }

        public async Task<Venta> UpdateAsync(Venta venta)
        {
            _context.Venta.Update(venta);
            await _context.SaveChangesAsync();
            return venta;
        }

        public async Task DeleteAsync(long id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta != null)
            {
                _context.Venta.Remove(venta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
