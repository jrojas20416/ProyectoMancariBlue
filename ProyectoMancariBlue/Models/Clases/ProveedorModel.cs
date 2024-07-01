using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using System.Collections.Immutable;

namespace ProyectoMancariBlue.Models.Clases
{
    public class ProveedorModel : IProveedor
    {
        private readonly MancariBlueContext _context;

        public ProveedorModel(MancariBlueContext context)
        {
            _context = context;
        }

        public IEnumerable<Proveedor> GetAll()
        {
            return _context.Proveedor
                .Include(p => p.CategoriaProveedor)
    .ToList();
        }
        public IEnumerable<Proveedor> GetAllByStatus(bool Estato)
        {
            return _context.Proveedor
                .Include(p => p.CategoriaProveedor).Where(p => p.Estado == Estato)
    .ToList();
        }
        public async Task<Proveedor> GetByIdAsync(int id)
        {
            return await _context.Proveedor
                .Include(p => p.CategoriaProveedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Proveedor> PostProveedor(Proveedor proveedor)
        {
            await _context.Proveedor.AddAsync(proveedor);
            var response=await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task<Proveedor> UpdateAsync(Proveedor proveedor)
        {
            _context.Proveedor.Update(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
        public Proveedor GetByIdentification(string identification)
        {
            return _context.Proveedor.Where(x=>x.Identificacion==identification)
                .Include(p => p.CategoriaProveedor)
    .FirstOrDefault();
        }
    }
}
