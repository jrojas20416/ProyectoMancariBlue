using ProyectoMancariBlue.Models.Obj;
using System.Threading.Tasks;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface ICategoriaProducto
    {
        Task<IEnumerable<CategoriaProducto>> GetAllAsync();
        Task<CategoriaProducto> GetByIdAsync(int id);
        Task AddAsync(CategoriaProducto categoriaProducto);
        Task UpdateAsync(CategoriaProducto categoriaProducto);
        Task DeleteAsync(int id);
    }
}
