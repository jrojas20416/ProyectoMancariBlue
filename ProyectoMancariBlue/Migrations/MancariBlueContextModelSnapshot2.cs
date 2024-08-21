using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ProyectoMancariBlue.Migrations
{
    public class MancariBlueContextModelSnapshot2 : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Animal", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                b.Property<bool>("Estado")
                    .HasColumnType("bit");

                b.Property<DateTime>("FechaNacimiento")
                    .HasColumnType("datetime2");

                b.Property<string>("Genero")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("Madre")
                    .HasColumnType("int");

                b.Property<int>("Padre")
                    .HasColumnType("int");

                b.Property<int>("PartosRegistrados")
                    .HasColumnType("int");

                b.Property<int>("PesoKG")
                    .HasColumnType("int");

                b.Property<string>("Raza")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Animal");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Empleado", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                b.Property<string>("Apellidos")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Canton")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Cedula")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Contrasena")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Correo")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Distrito")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<int>("Edad")
                    .HasColumnType("int");

                b.Property<bool>("Estado")
                    .HasColumnType("bit");

                b.Property<DateTime>("FechaIngreso")
                    .HasColumnType("datetime2");

                b.Property<long?>("IdRol")
                    .IsRequired()
                    .HasColumnType("bigint");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Provincia")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Puesto")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<bool>("RContrasena")
                    .HasColumnType("bit");

                b.Property<double>("Salario")
                    .HasColumnType("float");

                b.Property<int>("Telefono")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("IdRol");

                b.ToTable("Empleado");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Rol", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                b.Property<string>("Descripcion")
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnType("nvarchar(250)");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Rol");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Empleado", b =>
            {
                b.HasOne("ProyectoMancariBlue.Models.Obj.Rol", "Rol")
                    .WithMany()
                    .HasForeignKey("IdRol")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Rol");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.CategoriaProveedor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("Descripcion")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.HasKey("Id");

                b.ToTable("CategoriaProveedor");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Proveedor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("Identificacion")
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("Nombre")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<int>("Telefono")
                    .HasColumnType("int");

                b.Property<string>("Correo")
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)");

                b.Property<int>("IdCategoriaProveedor")
                    .HasColumnType("int");

                b.Property<bool>("Estado")
                    .HasColumnType("bit");

                b.HasKey("Id");

                b.HasIndex("IdCategoriaProveedor");

                b.ToTable("Proveedor");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.CategoriaProducto", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("Descripcion")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)");

                b.HasKey("Id");

                b.ToTable("CategoriaProducto");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Producto", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("Codigo")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<string>("Descripcion")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)");

                b.Property<int>("IdTipoProducto")
                    .HasColumnType("int");

                b.Property<int>("IdCategoriaProducto")
                    .HasColumnType("int");

                b.Property<int>("IdProveedor")
                    .HasColumnType("int");

                b.Property<int>("Stock")
                    .HasColumnType("int");

                b.Property<bool>("Estado")
                    .HasColumnType("bit");

                b.HasKey("Id");

                b.HasIndex("IdCategoriaProducto");

                b.HasIndex("IdProveedor");

                b.ToTable("Producto");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Producto", b =>
            {
                b.HasOne("ProyectoMancariBlue.Models.Obj.CategoriaProducto", "CategoriaProducto")
                    .WithMany("Productos")
                    .HasForeignKey("IdCategoriaProducto")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("ProyectoMancariBlue.Models.Obj.Proveedor", "Proveedor")
                    .WithMany("Productos")
                    .HasForeignKey("IdProveedor")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("CategoriaProducto");

                b.Navigation("Proveedor");
            });

            modelBuilder.Entity("ProyectoMancariBlue.Models.Obj.Proveedor", b =>
            {
                b.HasOne("ProyectoMancariBlue.Models.Obj.CategoriaProveedor", "CategoriaProveedor")
                    .WithMany("Proveedores")
                    .HasForeignKey("IdCategoriaProveedor")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("CategoriaProveedor");
            });

#pragma warning restore 612, 618
        }
    }
}
