using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Enum
{
    public enum  EProductType
    {
        [Display(Name = "Sin registrar", Description = "Registro para vincular animales de los que no se tenga registros en el sistema.")]
        Sin_registrar =0,

        [Display(Name = "Vacuna", Description = "Vacunas de tipo analgésicos")]
        VACUNA = 1,

        [Display(Name = "Alimento", Description = "Alimentos para animales.")]
        ALIMENTO = 2,
    }
}
