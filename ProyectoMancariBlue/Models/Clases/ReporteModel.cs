using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class ReporteModel : IReporteModel
    {

        private readonly MancariBlueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ReporteModel(MancariBlueContext context,  IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Reporte> CreateReporte(Reporte reporte)
        {
            try
            {
                var newReporte = await _context.Reporte.AddAsync(reporte);
                await _context.SaveChangesAsync();

                return newReporte.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Reporte> DeleteReporteID(long id)
        {
            var reporte = await _context.Reporte.FindAsync(id);

            if (reporte != null)
            {
                _context.Reporte.Remove(reporte);
                await _context.SaveChangesAsync();
                return reporte;
            }

            return null;
        }

        public async Task<Reporte> UpdateReporte(Reporte reporte)
        {
            var reporteExistente = await _context.Reporte.FindAsync(reporte.Id);

            if (reporteExistente != null)
            {
                if (!string.IsNullOrEmpty(reporte.CodigoCVO))
                {
                    reporteExistente.CodigoCVO = reporte.CodigoCVO;
                }

                if (!string.IsNullOrEmpty(reporte.CodigoTransporte))
                {
                    reporteExistente.CodigoTransporte = reporte.CodigoTransporte;
                }

                if (!string.IsNullOrEmpty(reporte.Transaccion.ToString()))
                {
                    reporteExistente.Transaccion = reporte.Transaccion;
                }

                if (!string.IsNullOrEmpty(reporte.Identificacion))
                {
                    reporteExistente.Identificacion = reporte.Identificacion;
                }

                if (!string.IsNullOrEmpty(reporte.NombreCliente))
                {
                    reporteExistente.NombreCliente = reporte.NombreCliente;
                }

                if (reporte.FechaCreacion != default(DateTime))
                {
                    reporteExistente.FechaCreacion = reporte.FechaCreacion;
                }

                await _context.SaveChangesAsync();
                return reporteExistente;
            }

            return null;
        }

        public async Task<Reporte> GetReporteById(long id)
        {
            return await _context.Reporte.FindAsync(id);
        }

        public List<Reporte> GetAllReporte()
        {
            return _context.Reporte.ToList();
        }

        public List<Reporte> GetVenta()
        {
            // Aquí puedes ajustar la lógica para filtrar o buscar ventas específicas si es necesario
            return _context.Reporte.Where(r => r.Transaccion != null).ToList();
        }
    }
}
