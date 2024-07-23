using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface ICantonModel
    {
        Task<IEnumerable<Canton>> GetAllAsync();
        Task<Canton> GetByIdAsync(int id);
        Task<IEnumerable<Canton>> GetByProvinciaIdAsync(int provinciaId);
    }
}
