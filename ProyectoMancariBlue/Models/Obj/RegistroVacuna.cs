using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace ProyectoMancariBlue.Models.Obj
{
    [Table("Registro_Vacuna")]
    public class RegistroVacuna
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
      
        [ForeignKey("AnimalObj")]
        public long? IdAnimal { get; set; }
        [ForeignKey("ProductoObj")]
        public int? IdProducto { get; set; }
       
        private DateTime? _fechaAplicacion;
        [Column("Fecha_Aplicacion")]
        public DateTime? FechaAplicacion
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
        [Column("Fecha_Creacion")]
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
        public virtual Animal AnimalObj { get; set; }
        public virtual Producto ProductoObj { get; set; }



    }
}
