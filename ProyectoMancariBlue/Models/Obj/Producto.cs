using ProyectoMancariBlue.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(200)]
        public string Codigo { get; set; }
        [StringLength(200)]
        public string Nombre { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }
        public EProductType IdTipoProducto { get; set; }
        public int IdCategoriaProducto { get; set; }
        public int IdProveedor { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
        [ForeignKey("IdCategoriaProducto")]
        public virtual CategoriaProducto CategoriaProducto { get; set; }
        [ForeignKey("IdProveedor")]
        public virtual Proveedor Proveedor { get; set; }
    }
}
