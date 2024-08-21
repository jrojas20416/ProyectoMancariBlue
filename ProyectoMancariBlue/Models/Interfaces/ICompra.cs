using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface ICompra
    {
        IEnumerable<Compra> GetAll();
        Task<Compra> GetByIdAsync(long id);
        Task<Compra> PostCompra(Compra compra);
        Task<Compra> UpdateAsync(Compra compra);
        Task DeleteAsync(long id);
    }
}
