using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class PrestacionRequest
    {
       public IEnumerable<EmpleadoDTO> ListaEmpleados { get; set; }
        public LiquidacionDTO Liquidacion { get; set; }
    }


    public class LiquidacionRequest { 
    
    
    }
}
