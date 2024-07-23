using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models
{
    public class MancariBlueContext : DbContext
    {
        public MancariBlueContext(DbContextOptions<MancariBlueContext> opciones)
            : base(opciones)
        {
        }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<CategoriaProveedor> CategoriaProveedor { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<CategoriaProducto> CategoriaProducto { get; set; }
        public DbSet<RegistroVacuna> RegistroVacuna { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Canton> Cantones { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<Vacacion> Vacacion { get; set; }
        public DbSet<HistoricoPago> HistoricoPago { get; set; }
        public DbSet<RegistroLiquidacion> RegistroLiquidacion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Rol>(r =>
            {
                r.HasKey(x => x.Id);
                r.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                r.Property(x => x.Descripcion).HasMaxLength(250);
            });
            modelBuilder.Entity<Empleado>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Cedula).IsRequired().HasMaxLength(50);
                u.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                u.Property(x => x.DiasDisponibles).HasColumnName("Dias_Disponibles");
                u.Property(x => x.Correo).IsRequired().HasMaxLength(50);
                u.Property(x => x.Edad).IsRequired();
                u.Property(x => x.Telefono).IsRequired();
                u.Property(x => x.ProvinciaId).IsRequired();
                u.Property(x => x.CantonId).IsRequired();
                u.Property(x => x.DistritoId).IsRequired();
                u.Property(x => x.Puesto).IsRequired().HasMaxLength(50);
                u.Property(x => x.FechaIngreso).IsRequired();
                u.Property(x => x.Salario).IsRequired();
                u.Property(x => x.RContrasena).IsRequired();
                u.Property(x => x.Contrasena).IsRequired();
                u.Property(x => x.Estado).IsRequired();
                u.Property(x => x.Nacionalidad).IsRequired();
                u.Property<long?>("IdRol")
              .HasColumnType("bigint");


              
                u.HasOne(x => x.Rol)
                    .WithMany()
                    .HasForeignKey(x => x.IdRol)
                    .OnDelete(DeleteBehavior.Restrict);
                u.HasOne(x => x.Provincia)
                  .WithMany()
                  .HasForeignKey(x => x.ProvinciaId)
                  .OnDelete(DeleteBehavior.Restrict);
                u.HasOne(x => x.Canton)
                .WithMany()
                .HasForeignKey(x => x.CantonId)
                .OnDelete(DeleteBehavior.Restrict);
                u.HasOne(x => x.Distrito)
                .WithMany()
                .HasForeignKey(x => x.DistritoId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Animal>(a =>
            {
                a.HasKey(x => x.Id);
                a.Property(x => x.Padre).IsRequired(false);
                a.Property(x => x.Madre).IsRequired(false);
                a.Property(x => x.Raza).IsRequired();
                a.Property(x => x.Genero).IsRequired();
                a.Property(x => x.PartosRegistrados).IsRequired();
                a.Property(x => x.PesoKG).IsRequired();
                a.Property(x => x.FechaNacimiento).IsRequired();
                a.Property(x => x.Estado).IsRequired();
                a.Property(x => x.Codigo).IsRequired();

                a.HasOne(animal => animal.PadreAnimal)
                    .WithMany()
                    .HasForeignKey(animal => animal.Padre)
                    .OnDelete(DeleteBehavior.Restrict);

                a.HasOne(animal => animal.MadreAnimal)
                    .WithMany()
                    .HasForeignKey(animal => animal.Madre)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CategoriaProveedor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Identificacion)
                    .HasMaxLength(100);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(200);
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Correo)
                    .HasMaxLength(150);
                entity.Property(e => e.Estado).IsRequired();

                entity.HasOne(e => e.CategoriaProveedor)
                    .WithMany(c => c.Proveedores)
                    .HasForeignKey(e => e.IdCategoriaProveedor)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200); 
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500); 
                entity.Property(e => e.IdCategoriaProducto).IsRequired();
                entity.Property(e => e.IdTipoProducto).IsRequired();
                entity.Property(e => e.IdProveedor).IsRequired();
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e => e.Estado).IsRequired();

                entity.HasOne(e => e.CategoriaProducto)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(e => e.IdCategoriaProducto)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Proveedor)
                   .WithMany(c => c.Productos)
                   .HasForeignKey(e => e.IdProveedor)
                   .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CategoriaProducto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
            });


            modelBuilder.Entity<RegistroVacuna>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaAplicacion).IsRequired();
                entity.Property(e => e.IdProducto).IsRequired();
                entity.Property(e => e.IdAnimal).IsRequired();

                entity.HasOne(e => e.AnimalObj)
                    .WithMany()
                    .HasForeignKey(e => e.IdAnimal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ProductoObj)
                    .WithMany()
                    .HasForeignKey(e => e.IdProducto)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.ProvinciaID);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.HasMany(e => e.Cantones)
                      .WithOne(c => c.Provincia)
                      .HasForeignKey(c => c.ProvinciaID);
            });

            modelBuilder.Entity<Canton>(entity =>
            {
                entity.HasKey(e => e.CantonID);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.HasMany(e => e.Distritos)
                      .WithOne(d => d.Canton)
                      .HasForeignKey(d => d.CantonID);
            });

            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.HasKey(e => e.DistritoID);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Vacacion>(v =>
            {
                v.HasKey(x => x.Id);
                v.Property(x => x.IdEmpleado).IsRequired();
                v.Property(x => x.FechaInicio).IsRequired();
                v.Property(x => x.FechaFinal).IsRequired();
                v.Property(x => x.DiasSolicitados).IsRequired();
                v.Property(x => x.FechaCreacion).HasColumnName("Fecha_creacion");

                v.HasOne(x => x.EmpleadoModel)
                    .WithMany()
                    .HasForeignKey(x => x.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HistoricoPago>(h =>
            {
                h.HasKey(x => x.Id);
                h.Property(x => x.Patrono).IsRequired().HasMaxLength(100);
                h.Property(x => x.SalarioBruto).IsRequired().HasColumnType("decimal(18,2)");
                h.Property(x => x.CCSS).IsRequired().HasColumnType("decimal(18,2)");
                h.Property(x => x.LPT).IsRequired().HasColumnType("decimal(18,2)");
                h.Property(x => x.TotalDeducciones).IsRequired().HasColumnType("decimal(18,2)");
                h.Property(x => x.SalarioNeto).IsRequired().HasColumnType("decimal(18,2)");
                h.Property(x => x.EmpleadoId).IsRequired();
                h.Property(x => x.FechaPago).HasColumnType("datetime");

                h.HasOne(x => x.Empleado)
                  .WithMany(e => e.HistoricoPagos)
                  .HasForeignKey(x => x.EmpleadoId)
                  .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<RegistroLiquidacion>(l =>
            {
                l.HasKey(x => x.Id);
                l.Property(x => x.FechaSalida).HasColumnType("datetime");
                l.Property(x => x.MotivoSalida).HasColumnType("int");
                l.Property(x => x.Preaviso).HasColumnType("bit");
                l.Property(x => x.AguinaldoPP).HasColumnType("decimal(28,8)");
                l.Property(x => x.PreavisoV).HasColumnType("decimal(28,8)");
                l.Property(x => x.Cesantia).HasColumnType("decimal(28,8)");
                l.Property(x => x.VacacionesNoGozadas).HasColumnType("decimal(28,8)");
                l.Property(x => x.TotalLiquidacion).HasColumnType("decimal(28,8)");

                l.HasOne(x => x.Empleado)
                  .WithMany(e => e.Liquidaciones)
                  .HasForeignKey(x => x.IdEmpleado)
                  .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
