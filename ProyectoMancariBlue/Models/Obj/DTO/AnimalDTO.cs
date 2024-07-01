using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ProyectoMancariBlue.Models.Enum;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class AnimalDTO
    {
        public long Id { get; set; }
        public long? Padre { get; set; }
        public long? Madre { get; set; }
        public ECattle Raza { get; set; }
        public string Genero { get; set; }
        [DisplayName("Partos registrados")]
        public int PartosRegistrados { get; set; }
        [DisplayName("Peso en KG")]
        public int PesoKG { get; set; }
        [DisplayName("Fecha nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }
        public string Codigo { get; set; }
        public AnimalDTO PadreAnimal { get; set; }
        public AnimalDTO MadreAnimal { get; set; }
    }
}
