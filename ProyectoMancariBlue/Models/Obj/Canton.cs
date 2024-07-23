namespace ProyectoMancariBlue.Models.Obj
{
    public class Canton
    {
        public int CantonID { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaID { get; set; }
        public Provincia Provincia { get; set; }
        public ICollection<Distrito> Distritos { get; set; }
    }
}
