using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "El campo Padre es requerido")]
        public int Padre { get; set; }

        [Required(ErrorMessage = "El campo Madre es requerido")]
        public int Madre { get; set; }

        [Required(ErrorMessage = "El campo Raza es requerido")]
        [StringLength(50)]
        public string Raza { get; set; }

        [Required(ErrorMessage = "El campo Genero es requerido")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El campo Partos Registrados es requerido")]
        [Display(Name = "Partos Registrados")]
        public int PartosRegistrados { get; set; }

        [Required(ErrorMessage = "El campo Peso es requerido")]
        [Display(Name = "Peso KG")]
        public int PesoKG { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)] // Esto indica que solo se debe manejar la fecha
        public DateTime FechaNacimiento
        {
            get { return _fechaNacimiento; }
            set { _fechaNacimiento = value.Date; } // Esto asegura que solo la parte de la fecha se almacene
        }
        private DateTime _fechaNacimiento;


        [Required(ErrorMessage = "El campo Estado es requerido")]
        public bool Estado { get; set; }
    }
}
