using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class ReporteDTO
    {
        public long? Id { get; set; }
        [DisplayName("CodigoCVO")]
        public string? CodigoCVO { get; set; }
        [DisplayName("CodigoTransporte")]
        public string? CodigoTransporte { get; set; }
        [DisplayName("Transaccion")]
        public long? Transaccion { get; set; }
        [DisplayName("Identificacion")]
        public string? Identificacion { get; set; }
        [DisplayName("NombreCliente")]
        public string? NombreCliente { get; set; }

        [DisplayName("FechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        public VentaDTO? Venta { get; set; }

    }
}
