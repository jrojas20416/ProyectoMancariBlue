using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Enum
{
    public enum EReasonDeparture
    {
        [Display(Name = "Despido Responsabilidad Patronal", Description = "Despido con responsabilidad del empleador.")]
        DespidoResponsabilidadPatronal = 0,

        [Display(Name = "Despido Sin Responsabilidad Patronal", Description = "Despido sin responsabilidad del empleador.")]
        DespidoSinResponsabilidadPatronal = 1,

        [Display(Name = "Renuncia", Description = "Renuncia voluntaria del empleado.")]
        Renuncia = 2,

        [Display(Name = "Pensión", Description = "Salida por jubilación o pensión.")]
        Pension = 3
    }
}
