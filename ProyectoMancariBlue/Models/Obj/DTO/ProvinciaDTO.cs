namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class ProvinciaDTO
    {
        public int ProvinciaID { get; set; }
        public string Nombre { get; set; }
        public ICollection<CantonDTO> Cantones { get; set; }
    }
}
