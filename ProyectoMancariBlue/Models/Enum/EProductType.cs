using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Enum
{
    public enum  EProductType
    {
        [Display(Name = "Sin registrar", Description = "Registro para vincular animales de los que no se tenga registros en el sistema.")]
        Sin_registrar =0,

        [Display(Name = "Vacunas", Description = "Vacunas varias para animales")]
        Vacunas = 1,

        [Display(Name = "Alimentos", Description = "Alimentos varias para animales")]
        Alimentos = 2,

        [Display(Name = "Animales", Description = "Animales")]
        Animales = 3,

        [Display(Name = "Medicamentos", Description = "Medicamentos varios para animales")]
        Medicamentos = 4,

        [Display(Name = "Materia Prima", Description = "Materia varia para animales")]
        Materia_prima = 5,

        [Display(Name = "Materiales", Description = "Materiales varios para construcción")]
        Materiales= 6,

        [Display(Name = "Herramientas", Description = "Herramientas varias para trabajo")]
        Herramientas = 7,
    }
}
