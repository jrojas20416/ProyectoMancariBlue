using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoMancariBlue.Models.Obj
{
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "El campo Cédula es requerido")]
        [StringLength(50)]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50)]
        public string Nombre { get; set; }

  

        [Required(ErrorMessage = "El campo Correo es requerido")]
        [StringLength(50)]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo Nacionalidad es requerido")]
        [StringLength(50)]
        public string Nacionalidad { get; set; }


        [Required(ErrorMessage = "El campo Edad es requerido")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo Telefono es requerido")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El campo Provincia es requerido")]
        [ForeignKey("Provincia")]
        public int ProvinciaId { get; set; }
        [Required(ErrorMessage = "El campo Cantón es requerido")]
        [ForeignKey("Canton")]
        public int CantonId { get; set; }

        [Required(ErrorMessage = "El campo Distrito es requerido")]
        [ForeignKey("Distrito")]
        public int DistritoId { get; set; }

        [Required(ErrorMessage = "El campo Puesto es requerido")]
        [StringLength(50)]
        public string Puesto { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Ingreso es requerida")]
        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)] // Esto indica que solo se debe manejar la fecha
        public DateTime FechaIngreso
        {
            get { return _fechaIngreso; }
            set { _fechaIngreso = value.Date; } // Esto asegura que solo la parte de la fecha se almacene
        }
        private DateTime _fechaIngreso;

        [Required(ErrorMessage = "El campo Salario es requerido")]
        public double Salario { get; set; }

        [Required(ErrorMessage = "El campo RContrasena es requerido")]
        public bool RContrasena { get; set; }

        [Required(ErrorMessage = "El campo Contrasena es requerido")]
        [StringLength(50)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [ForeignKey("IdRol")]
        [Required(ErrorMessage = "Se debe seleccionar un Rol")]
        public long? IdRol { get; set; }
        public Rol Rol { get; set; }

        public virtual Provincia Provincia { get; set; }
        public virtual Canton Canton { get; set; }
        public virtual Distrito Distrito { get; set; }  
        [Required(ErrorMessage = "El campo Estado es requerido")]
        public bool Estado { get; set; }
        [Column("Dias_Disponibles")] 
        
        public int DiasDisponibles { get; set; }

        public ICollection<HistoricoPago> HistoricoPagos { get; set; }

        public ICollection<RegistroLiquidacion> Liquidaciones { get; set; }
    }

      
    
    public class EmpleadoLogin
    {

        [Required(ErrorMessage = "El campo Contrasena es requerido")]
        [StringLength(50)]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public string Empleado { get; set; }
    }

    public class RestorePassword
    {
        [Required(ErrorMessage = "El campo Correo es requerido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo Cedula es requerido")]

        public string Cedula { get; set; }
    }

    public class EmpleadoInicio
    {
        public EmpleadoLogin EmpleadoLogin { get; set; }
        public RestorePassword RestorePassword { get; set; }
    }
    public class CambiarContrasena
    {

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña Actual")]
        public string oldPassword { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string password { get; set; }

        [Display(Name = "Confirnar Contraseña")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string confirmPassword { get; set; }
    }
}

