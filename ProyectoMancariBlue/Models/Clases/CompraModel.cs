using ProyectoMancariBlue.Models.Obj;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;

namespace ProyectoMancariBlue.Models.Clases
{
    public class CompraModel : ICompra
    {
        private readonly MancariBlueContext _context;

        public CompraModel(MancariBlueContext context)
        {
            _context = context;
        }

        public IEnumerable<Compra> GetAll()
        {
            return _context.Compra
                    .Include(c => c.Proveedor)
                .Include(c => c.Producto)
                .Include(c => c.Animal)
                .ToList();
        }

        public async Task<Compra> GetByIdAsync(long id)
        {
            return await _context.Compra
                .Include(c => c.Proveedor)
                .Include(c => c.Producto)
                .Include(c => c.Animal)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Compra> PostCompra(Compra compra)
        {
            await _context.Compra.AddAsync(compra);
            await _context.SaveChangesAsync();
            return compra;
        }

        public async Task<Compra> UpdateAsync(Compra compra)
        {
            _context.Compra.Update(compra);
            await _context.SaveChangesAsync();
            return compra;
        }

        public async Task DeleteAsync(long id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra != null)
            {
                _context.Compra.Remove(compra);
                await _context.SaveChangesAsync();
            }
        }
    }
}
