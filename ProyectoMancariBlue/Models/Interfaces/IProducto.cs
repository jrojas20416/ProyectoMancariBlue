using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IProducto
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto> GetByIdAsync(int id);
        Producto Add(Producto producto);
        Producto Update(Producto producto);
        Task DeleteAsync(int id);
    }
}
