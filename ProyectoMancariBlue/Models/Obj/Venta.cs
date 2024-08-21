using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string? IdAnimal { get; set; }

        [StringLength(100)]
        public string CedulaCliente { get; set; }

        [StringLength(100)]
        public string NombreCliente { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column(TypeName = "decimal(28,8)")]
        public decimal Importe { get; set; }

        public DateTime FechaVenta { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
