using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Liquidacion
    {
     

        [Required(ErrorMessage = "El campo IdEmpleado es requerido")]
        [ForeignKey("Empleado")]
        public long IdEmpleado { get; set; }

        [Required(ErrorMessage = "El campo FechaSalida es requerido")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El campo MotivoSalida es requerido")]
        public int MotivoSalida { get; set; }

        [Required(ErrorMessage = "El campo Preaviso es requerido")]
        public bool Preaviso { get; set; }

        public Empleado Empleado { get; set; }
    }
}
