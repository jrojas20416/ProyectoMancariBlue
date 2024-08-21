using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IReporteModel
    {
        Task<Reporte> CreateReporte(Reporte reporte);

        Task<Reporte> DeleteReporteID(long id);

        Task<Reporte> UpdateReporte(Reporte reporte);

        Task<Reporte> GetReporteById(long id);

        List<Reporte> GetAllReporte();

        List<Reporte> GetVenta();
    }
}
