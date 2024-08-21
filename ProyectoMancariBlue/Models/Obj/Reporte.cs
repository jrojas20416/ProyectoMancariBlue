using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Reporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "El campo Código CVO es requerido")]
        [StringLength(50)]
        public string CodigoCVO { get; set; }

        [Required(ErrorMessage = "El campo Código Transporte es requerido")]
        [StringLength(50)]
        public string CodigoTransporte { get; set; }

        [Required(ErrorMessage = "El campo Transacción es requerido")]
        [ForeignKey("VentaModel")]
        public long Transaccion { get; set; }

        [Required(ErrorMessage = "El campo Identificación es requerido")]
        [StringLength(50)]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El campo Nombre Cliente es requerido")]
        [StringLength(100)]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Creación es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public virtual Venta VentaModel { get; set; }
    }
}
