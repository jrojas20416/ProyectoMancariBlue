using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IVenta
    {
        IEnumerable<Venta> GetAll();
        Task<Venta> GetByIdAsync(long id);
        Task<Venta> PostVenta(Venta venta);
        Task<Venta> UpdateAsync(Venta venta);
        Task DeleteAsync(long id);
    }
}
