using ProyectoMancariBlue.Models.Enum;
using System.ComponentModel;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class CompraDTO
    {
        public long? Id { get; set; }

        [DisplayName("Tipo de Producto")]
        public EProductType? IdTipoProducto { get; set; }

        [DisplayName("Proveedor")]
        public int? IdProveedor { get; set; }

        [DisplayName("Producto")]
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }

        [DisplayName("Animal")]
        public int? IdAnimal { get; set; }

        public string? Descripcion { get; set; }

        public decimal? Importe { get; set; }

        [DisplayName("Fecha de Compra")]
        public DateTime? FechaCompra { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public ProveedorDTO? Proveedor { get; set; }

        public ProductoDto? Producto { get; set; }

        public AnimalDTO? Animal
        {
            get; set;
        }
    }
}
