using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoMancariBlue.Models.Clases
{
    public class HistoricoPagoModel : IHistoricoPagoModel
    {
        private readonly MancariBlueContext _context;

        public HistoricoPagoModel(MancariBlueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoricoPago>> GetAllAsync()
        {
            return await _context.HistoricoPago.ToListAsync();
        }

        public async Task<HistoricoPago> GetByIdAsync(long id)
        {
            return await _context.HistoricoPago.FindAsync(id);
        }

        public async Task<HistoricoPago> AddAsync(HistoricoPago historicoPago)
        {
            await _context.HistoricoPago.AddAsync(historicoPago);
            await _context.SaveChangesAsync();
            return historicoPago;
        }

        public async Task<HistoricoPago> UpdateAsync(HistoricoPago historicoPago)
        {
            _context.HistoricoPago.Update(historicoPago);
            await _context.SaveChangesAsync();
            return historicoPago;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var historicoPago = await _context.HistoricoPago.FindAsync(id);
            if (historicoPago == null)
            {
                return false;
            }

            _context.HistoricoPago.Remove(historicoPago);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<HistoricoPago>> GetPagosByEmpleadoIdAsync(long empleadoId)
        {
            return await _context.HistoricoPago
                .Where(p => p.EmpleadoId == empleadoId)
                  .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistoricoPago>> GetPagosDesdeDiciembre(long empleadoId)
        {
            DateTime now = DateTime.Now;
            int currentYear = now.Year;
            int currentMonth = now.Month;

            DateTime startDate;
            if (currentMonth >= 12)
            {
                startDate = new DateTime(currentYear, 12, 1);
            }
            else
            {
                startDate = new DateTime(currentYear - 1, 12, 1);
            }
            var pagos = await _context.HistoricoPago
                .Where(p => p.EmpleadoId == empleadoId && p.FechaPago >= startDate)
                .OrderBy(p => p.FechaPago)
                .ToListAsync();

            return pagos;
        }
    }
}
