using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class HistoricoPagoDTO
    {
        public int? Id { get; set; }

       
        public string? Patrono { get; set; }

        
        public decimal? SalarioBruto { get; set; }

        
        public decimal? CCSS { get; set; }

        
        public decimal? LPT { get; set; }

        
        public decimal? TotalDeducciones { get; set; }

        
        public decimal? SalarioNeto { get; set; }

        
        public long? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
