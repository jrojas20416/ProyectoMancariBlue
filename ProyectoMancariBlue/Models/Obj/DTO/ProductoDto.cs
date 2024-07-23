using ProyectoMancariBlue.Models.Enum;
using System.ComponentModel;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class ProductoDto
    {
        public int Id { get; set; }
        [DisplayName("Código producto")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public EProductType IdTipoProducto { get; set; }
        public int IdCategoriaProducto { get; set; }
        public int IdProveedor { get; set; }
        [DisplayName("Disponible")]
        public int Stock { get; set; }
        public string Identification { get; set; }
        public bool Estado { get; set; }
        public CategoriaProductoDto CategoriaProducto { get; set; }
        public ProveedorDTO Proveedor { get; set; }
    }
}
