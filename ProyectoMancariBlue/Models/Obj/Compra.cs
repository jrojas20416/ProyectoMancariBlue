using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProyectoMancariBlue.Models.Enum;

namespace ProyectoMancariBlue.Models.Obj
{
    [Table("Compra")]
    public class Compra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public EProductType IdTipoProducto { get; set; }

        public int? IdProveedor { get; set; }

        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }
        public long? IdAnimal { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Column(TypeName = "decimal(28,8)")]
        public decimal Importe { get; set; }

        public DateTime FechaCompra { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdProveedor")]
        public virtual Proveedor? Proveedor { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("IdAnimal")]
        public virtual Animal? Animal { get; set; }
    }
}
