using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IProvinciaModel
    {
        Task<IEnumerable<Provincia>> GetAllAsync();
        Task<Provincia> GetByIdAsync(int id);
    }
}
