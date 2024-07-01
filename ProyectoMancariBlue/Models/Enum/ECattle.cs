using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Enum
{
    public enum ECattle
    {
        [Display(Name = "Sin registrar", Description = "Registro para vincular animales de los que no se tenga registros en el sistema.")]
        Sin_registrar =0,

        [Display(Name = "Hereford", Description = "Originaria de Inglaterra, conocida por su carne de alta calidad")]
        Hereford = 1,

        [Display(Name = "Angus", Description = "Originaria de Escocia, apreciada por su carne tierna y marmoleada")]
        Angus = 2,

        [Display(Name = "Holstein", Description = "También conocida como Friesian, es la raza de vacas lecheras más común en muchos países")]
        Holstein = 3,

        [Display(Name = "Brahman", Description = "Originaria de India, es resistente al calor y a las enfermedades")]
        Brahman = 4,

        [Display(Name = "Charolais", Description = "Originaria de Francia, conocida por su gran tamaño y carne de alta calidad")]
        Charolais = 5,

        [Display(Name = "Limousin", Description = "Otra raza francesa, apreciada por su carne magra y su crecimiento rápido")]
        Limousin = 6,

        [Display(Name = "Simmental", Description = "Originaria de Suiza, es una raza versátil utilizada tanto para la carne como para la leche")]
        Simmental = 7,

        [Display(Name = "Jersey", Description = "Originaria de la Isla de Jersey, conocida por su alta producción de leche con alto contenido de grasa")]
        Jersey = 8,

        [Display(Name = "Gyr", Description = "Otra raza india, conocida por su resistencia y alta producción de leche en climas cálidos")]
        Gyr = 9,

     
    }
}
