using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class ReporteRequest
    {
        public IEnumerable<ReporteDTO> listaReporte { get; set; }
        public IEnumerable<VentaDTO> listaTransacciones { get; set; }
        public ReporteDTO ReporteCreate { get; set; }
        public ReporteDTO ReporteModify { get; set; }

    }
}
