using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class TransaccionRequest
    {
        public List<CompraDTO> CompraDTOs { get; set;}

        public List<VentaDTO> ventaDTOs { get; set; }
        public List<ProveedorDTO> proveedorDTOs { get; set; }
        public List<AnimalDTO>VentaListaAnimal { get; set; }

        public CompraDTO CompraCreate { get; set; }
        public VentaDTO VentaCreate { get; set; }
    }
}
