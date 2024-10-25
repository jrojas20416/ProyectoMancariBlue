using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoMancariBlue.Models.Obj.DTO
{
    public class RegistroVacunaDTO
    {
      
        public long Id { get; set; }

        [Required]
        public long? IdAnimal { get; set; }
        [Required]
        public int? IdProducto { get; set; }
        private DateTime? _fechaAplicacion;
        [Required]
        [DisplayName("Fecha de aplicación")]
        public DateTime?FechaAplicacion
        {
            get => _fechaAplicacion;
            set
            {
                if (value.HasValue)
                {
                    _fechaAplicacion = value.Value.Date;
                }
                else
                {
                    _fechaAplicacion = null;
                }
            }
        }

        private DateTime? _fechaCreacion;
        
        public DateTime? FechaCreacion
        {
            get => _fechaCreacion;
            set
            {
                if (value.HasValue)
                {
                    _fechaCreacion = value.Value.Date;
                }
                else
                {
                    _fechaCreacion = null;
                }
            }
        }

        public virtual AnimalDTO AnimalObj { get; set; }
        public virtual ProductoDto ProductoObj { get; set; }

      
    }
}
