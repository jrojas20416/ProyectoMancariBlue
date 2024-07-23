namespace ProyectoMancariBlue.Models.Obj
{
    public class Distrito
    {
        public int DistritoID { get; set; }
        public string Nombre { get; set; }
        public int CantonID { get; set; }
        public Canton Canton { get; set; }
    }
}
