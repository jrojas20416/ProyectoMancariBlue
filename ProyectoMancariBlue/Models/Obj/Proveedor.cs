using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Identificacion { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        public int Telefono { get; set; }

        [StringLength(150)]
        public string Correo { get; set; }

        public int IdCategoriaProveedor { get; set; }
        public bool Estado { get; set; }


        [ForeignKey("IdCategoriaProveedor")]
        public virtual CategoriaProveedor CategoriaProveedor { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
