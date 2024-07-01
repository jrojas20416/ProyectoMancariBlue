using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IProveedor
    {
        IEnumerable<Proveedor> GetAll();
        Task<Proveedor> GetByIdAsync(int id);
        Task<Proveedor> PostProveedor(Proveedor proveedor);
        Task<Proveedor> UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
        Proveedor GetByIdentification(string identification);
         IEnumerable<Proveedor> GetAllByStatus(bool Estato);
    }
}
