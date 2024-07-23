using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class VacunaRequest
    {

        public AnimalDTO AnimalObjView { get; set; }
        public List<RegistroVacunaDTO> ListaRegistroVacunaView { get; set; }
        public List<ProductoDto> ListaProductoView { get; set; }
        public List<AnimalDTO> ListaAnimalView { get; set; }
        public RegistroVacunaDTO RegistroVacuna { get; set;}
        public RegistroVacunaDTO RegistroVacunaModifyModal { get; set; }
    }
}
