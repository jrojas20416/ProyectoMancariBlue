using System.ComponentModel;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class CategoriaProveedorDTO
    {
        public int Id { get; set; }
       [DisplayName("Categoria proveedor")]
        public string Descripcion { get; set; }
        public List<ProveedorDTO> Proveedores { get; set; }
    }
}
