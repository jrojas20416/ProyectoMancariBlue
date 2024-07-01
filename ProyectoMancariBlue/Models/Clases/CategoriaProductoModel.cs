using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class CategoriaProductoModel : ICategoriaProducto
    {
        private readonly MancariBlueContext _context;

        public CategoriaProductoModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaProducto>> GetAllAsync()
        {
            return await _context.CategoriaProducto.ToListAsync();
        }

        public async Task<CategoriaProducto> GetByIdAsync(int id)
        {
            return await _context.CategoriaProducto.FindAsync(id);
        }

        public async Task AddAsync(CategoriaProducto categoriaProducto)
        {
            _context.CategoriaProducto.Add(categoriaProducto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoriaProducto categoriaProducto)
        {
            _context.Entry(categoriaProducto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categoriaProducto = await _context.CategoriaProducto.FindAsync(id);
            if (categoriaProducto != null)
            {
                _context.CategoriaProducto.Remove(categoriaProducto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
