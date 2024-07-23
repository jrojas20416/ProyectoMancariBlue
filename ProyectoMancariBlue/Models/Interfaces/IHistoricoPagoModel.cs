using ProyectoMancariBlue.Models.Obj;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IHistoricoPagoModel
    {
        Task<IEnumerable<HistoricoPago>> GetAllAsync();
        Task<HistoricoPago> GetByIdAsync(long id);
        Task<HistoricoPago> AddAsync(HistoricoPago historicoPago);
        Task<HistoricoPago> UpdateAsync(HistoricoPago historicoPago);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<HistoricoPago>> GetPagosByEmpleadoIdAsync(long empleadoId);
        Task<IEnumerable<HistoricoPago>> GetPagosDesdeDiciembre(long empleadoId);
    }
}
