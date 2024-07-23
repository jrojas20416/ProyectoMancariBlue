using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IDistritoModel
    {
        Task<IEnumerable<Distrito>> GetAllAsync();
        Task<Distrito> GetByIdAsync(int id);
        Task<IEnumerable<Distrito>> GetByCantonIdAsync(int cantonId);
    }
}
