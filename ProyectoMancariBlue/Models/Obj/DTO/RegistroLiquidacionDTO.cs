namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class RegistroLiquidacionDTO:LiquidacionDTO
    {
        public long Id { get; set; }

        public decimal AguinaldoPP { get; set; }

        public decimal PreavisoV { get; set; }

        public decimal Cesantia { get; set; }

        public decimal VacacionesNoGozadas { get; set; }

        public decimal TotalLiquidacion { get; set; }
    }
}
