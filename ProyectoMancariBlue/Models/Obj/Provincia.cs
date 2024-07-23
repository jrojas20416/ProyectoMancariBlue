namespace ProyectoMancariBlue.Models.Obj
{
    public class Provincia
    {
        public int ProvinciaID { get; set; }
        public string Nombre { get; set; }
        public ICollection<Canton> Cantones { get; set; }
    }
}
