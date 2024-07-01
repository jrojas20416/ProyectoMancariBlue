using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
  public class ProveedorDTO
    {
        public int Id { get; set; }
        [DisplayName("Identificación")]
        public string Identificacion { get; set; }
        [DisplayName("Proveedor")]
        public string Nombre { get; set; }
        [DisplayName("Teléfono")]
        public int Telefono { get; set; }
        public string Correo { get; set; }
        public int IdCategoriaProveedor { get; set; }
        public bool Estado { get; set; }
        public CategoriaProveedorDTO CategoriaProveedor { get; set; }
        public  Proveedor Proveedor { get; set; }
    }
}
