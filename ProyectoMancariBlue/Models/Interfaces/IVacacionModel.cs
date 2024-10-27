using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IVacacionModel
    {
        Task<IEnumerable<Vacacion>> GetAllAsync();
        Task<Vacacion> GetByIdAsync(int id);
        Task AddAsync(Vacacion vacacion);
        Task UpdateAsync(Vacacion vacacion);

        Task DeleteAsync(int id);
    }
}
