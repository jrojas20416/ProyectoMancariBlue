using System.ComponentModel;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class VentaDTO
    {
        public long Id { get; set; }

        [DisplayName("Animal")]
        public string? IdAnimal { get; set; }

        [DisplayName("Cédula del Cliente")]
        public string? CedulaCliente { get; set; }
        [DisplayName("Descripción")]
        public string? Descripcion { get; set; }

        [DisplayName("Nombre del Cliente")]
        public string? NombreCliente { get; set; }

        public decimal? Importe { get; set; }

        [DisplayName("Fecha de Venta")]
        public DateTime? FechaVenta { get; set; }
        public DateTime? FechaCreacion { get; set; }


    }
}
