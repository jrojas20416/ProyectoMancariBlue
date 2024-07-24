using ProyectoMancariBlue.Models.Enum;
using System.Text.Json.Serialization;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class LiquidacionDTO
    {
        public long? IdEmpleado { get; set; }
        public DateTime? FechaSalida { get; set; }
        public EReasonDeparture? MotivoSalida { get; set; }
        public bool Preaviso { get; set; }
        public EmpleadoDTO? empleadoObj { get; set; }


    }
}
