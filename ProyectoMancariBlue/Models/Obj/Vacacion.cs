using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Vacacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo IdEmpleado es requerido")]
        [ForeignKey("EmpleadoModel")]
        public long IdEmpleado { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Inicio es requerido")]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value.Date; }
        }
        private DateTime _fechaInicio;

        [Required(ErrorMessage = "El campo Fecha Final es requerido")]
        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal
        {
            get { return _fechaFinal; }
            set { _fechaFinal = value.Date; }
        }
        private DateTime _fechaFinal;
    
        [Display(Name = "Fecha creación")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion        {
            get { return _FechaCreacion; }
            set { _FechaCreacion = value.Date; }
        }
        private DateTime _FechaCreacion;

        [Required(ErrorMessage = "El campo Dias Solicitados es requerido")]
        [Display(Name = "Dias Solicitados")]
        public int DiasSolicitados { get; set; }
        public virtual Empleado EmpleadoModel { get; set; }
    }
}
