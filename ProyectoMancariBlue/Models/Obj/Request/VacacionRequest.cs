using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class VacacionRequest
    {
        public IEnumerable<VacacionDTO> VacacionLista { get; set; }
        public VacacionDTO VacacionCreate { get; set; }
        public VacacionDTO VacacionModify { get; set; }
        public IEnumerable<EmpleadoDTO> EmpleadoLista { get; set; }
    }
}
