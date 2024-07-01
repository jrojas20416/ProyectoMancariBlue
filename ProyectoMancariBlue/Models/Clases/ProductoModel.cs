using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class ProductoModel : IProducto
    {
        private readonly MancariBlueContext _context;

        public ProductoModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Producto.Include(p => p.CategoriaProducto).Include(p=>p.Proveedor).ToListAsync();
        }

        public async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Producto.Include(p => p.CategoriaProducto)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public  Producto Add(Producto producto)
        {
            try {
           

                _context.Producto.Add(producto);
             _context.SaveChanges();
            return producto;
        }
        catch (DbUpdateException dbEx)
        {
            
            Console.WriteLine($"Error al insertar el producto: {dbEx.Message}");
            throw;
        }
}

        public  Producto Update(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
             _context.SaveChanges();
            return producto;
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
