using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class EmpleadoDTO
    {
        public long? Id { get; set; }
        [DisplayName("Cédula")]
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
       public string? Correo { get; set; }
        public int?   Edad { get; set; }

       
        public int? Telefono { get; set; }

       
        public int? ProvinciaId { get; set; }

        
        public int? CantonId { get; set; }

       
        public int? DistritoId { get; set; }
        public int? DiasDisponibles { get; set; }


        public string? Puesto { get; set; }

      
        public DateTime?  FechaIngreso { get; set; }

       
        public double? Salario { get; set; }

        
        public bool? RContrasena { get; set; }
        public string? Contrasena { get; set; }
        public string? Nacionalidad { get; set; }

        public long? IdRol { get; set; }
        public Rol? Rol { get; set; }
        public bool Estado { get; set; }
        public bool UsuarioSistema { get; set; }
        public ProvinciaDTO? Provincia { get; set; }
        public CantonDTO? Canton { get; set; }
        public DistritoDTO? Distrito { get; set; }
    }
}
