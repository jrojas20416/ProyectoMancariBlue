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
                u.Property(x => x.Apellidos).IsRequired().HasMaxLength(50);
                u.Property(x => x.Correo).IsRequired().HasMaxLength(50);
                u.Property(x => x.Edad).IsRequired();
                u.Property(x => x.Telefono).IsRequired();
                u.Property(x => x.Provincia).IsRequired().HasMaxLength(50);
                u.Property(x => x.Canton).IsRequired().HasMaxLength(50);
                u.Property(x => x.Distrito).IsRequired().HasMaxLength(50);
                u.Property(x => x.Puesto).IsRequired().HasMaxLength(50);
                u.Property(x => x.FechaIngreso).IsRequired();
                u.Property(x => x.Salario).IsRequired();
                u.Property(x => x.RContrasena).IsRequired();
                u.Property(x => x.Contrasena).IsRequired();
                u.Property(x => x.Estado).IsRequired();
                u.Property<long?>("IdRol")
              .HasColumnType("bigint");


                // Configurar relación con la tabla Rol (llave foránea)
                u.HasOne(x => x.Rol)
                    .WithMany()
                    .HasForeignKey(x => x.IdRol)
                    .OnDelete(DeleteBehavior.Restrict); // Opciones de eliminación (puedes ajustarlo según tu lógica)
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


        }

    }
}
