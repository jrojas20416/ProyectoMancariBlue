using Org.BouncyCastle.Asn1.Mozilla;
using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Models.Obj.Request
{
    public class AnimalRequest
    {
        public List<AnimalDTO> Fathers { get; set; }
        public List<AnimalDTO> Mothers { get; set; }
    }
}
