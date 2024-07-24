namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class VacacionDTO
    {
        public int? Id { get; set; }
        public long? IdEmpleado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? DiasSolicitados { get; set; }

        public EmpleadoDTO? Empleado { get; set; }
    }
}
