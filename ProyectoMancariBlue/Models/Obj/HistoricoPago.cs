using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class HistoricoPago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Patrono es requerido")]
        [StringLength(100)]
        public string Patrono { get; set; }

        [Required(ErrorMessage = "El campo SalarioBruto es requerido")]
        public decimal SalarioBruto { get; set; }

        [Required(ErrorMessage = "El campo CCSS es requerido")]
        public decimal CCSS { get; set; }

        [Required(ErrorMessage = "El campo LPT es requerido")]
        public decimal LPT { get; set; }

        [Required(ErrorMessage = "El campo TotalDeducciones es requerido")]
        public decimal TotalDeducciones { get; set; }

        [Required(ErrorMessage = "El campo SalarioNeto es requerido")]
        public decimal SalarioNeto { get; set; }

        [Required(ErrorMessage = "El campo EmpleadoId es requerido")]
        [ForeignKey("Empleado")]
        public long EmpleadoId { get; set; }

       
        public Empleado Empleado { get; set; }

        public DateTime? FechaPago { get; set; }
    }
}
