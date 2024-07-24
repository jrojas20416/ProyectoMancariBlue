namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class CantonDTO
    {
        public int CantonID { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaID { get; set; }
        public ProvinciaDTO Provincia { get; set; }
        public ICollection<DistritoDTO> Distritos { get; set; }
    }
}
