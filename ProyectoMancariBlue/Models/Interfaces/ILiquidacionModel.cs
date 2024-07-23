using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface ILiquidacionModel    {
        Task<IEnumerable<RegistroLiquidacion>> GetAllAsync();
        Task<RegistroLiquidacion> GetByIdAsync(long id);
        Task<RegistroLiquidacion> GetByIdEmpleadoAsync(long id);
        Task AddAsync(RegistroLiquidacion registroLiquidacion);
        Task UpdateAsync(RegistroLiquidacion registroLiquidacion);
        Task DeleteAsync(long id);

    }
}
