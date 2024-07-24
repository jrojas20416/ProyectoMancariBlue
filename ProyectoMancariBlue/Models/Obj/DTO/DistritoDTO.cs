namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class DistritoDTO
    {
        public int DistritoID { get; set; }
        public string Nombre { get; set; }
        public int CantonID { get; set; }
        public CantonDTO Canton { get; set; }
    }
}
