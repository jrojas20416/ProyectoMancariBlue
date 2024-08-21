using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProyectoMancariBlue.Models.Enum;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "El campo Padre es requerido")]
        [ForeignKey("PadreAnimal")]
        public long? Padre { get; set; }

        [Required(ErrorMessage = "El campo Madre es requerido")]
        [ForeignKey("MadreAnimal")]
        public long? Madre { get; set; }

        [Required(ErrorMessage = "El campo Raza es requerido")]
        [StringLength(50)]
        public ECattle Raza { get; set; }

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
            set { _fechaNacimiento = value.Date; } 
        }
        private DateTime _fechaNacimiento;


        [Display(Name = "Fecha de ultima vacuna")]
        [DataType(DataType.Date)]
        public DateTime? FechaUltimaVacuna
        {
            get { return _fechaUltimaVacuna; }
            set { _fechaUltimaVacuna = value.HasValue ? value.Value.Date : (DateTime?)null; }
        }
        private DateTime? _fechaUltimaVacuna;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        public bool Estado { get; set; }
        [Required(ErrorMessage = "El campo código es requerido")]
        public string Codigo { get; set; }

       
        public virtual Animal PadreAnimal { get; set; }
        public virtual Animal MadreAnimal { get; set; }

    }
}
