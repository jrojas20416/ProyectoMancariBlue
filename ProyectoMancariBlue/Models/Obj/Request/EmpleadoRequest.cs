using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class EmpleadoRequest
    {
        public IEnumerable<EmpleadoDTO> Empleado { get; set; }
        public IEnumerable<ProvinciaDTO> ListaProvincia { get; set; }
        public IEnumerable<CantonDTO> ListaCanton { get; set; }
        public IEnumerable<DistritoDTO> ListaDistrito { get; set; }
        public IEnumerable<Rol> ListaRol { get; set; }
        public EmpleadoDTO EmpleadoCreate { get; set; }
        public EmpleadoDTO EmpleadoModify { get; set; }
    }
}
