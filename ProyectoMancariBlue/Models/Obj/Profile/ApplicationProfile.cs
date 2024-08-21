using AutoMapper;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Animal, AnimalDTO>()
            .ForMember(dest => dest.PadreAnimal, opt => opt.MapFrom(src => src.PadreAnimal != null ? new AnimalDTO { Id = src.PadreAnimal.Id, Raza = src.PadreAnimal.Raza, Genero = src.PadreAnimal.Genero, Codigo = src.PadreAnimal.Codigo, FechaUltimaVacuna=src.FechaUltimaVacuna } : null))
            .ForMember(dest => dest.MadreAnimal, opt => opt.MapFrom(src => src.MadreAnimal != null ? new AnimalDTO { Id = src.MadreAnimal.Id, Raza = src.MadreAnimal.Raza, Genero = src.MadreAnimal.Genero, Codigo = src.MadreAnimal.Codigo, FechaUltimaVacuna = src.FechaUltimaVacuna } : null));

        CreateMap<AnimalDTO, Animal>()
            .ForMember(dest => dest.PadreAnimal, opt => opt.Ignore())
            .ForMember(dest => dest.MadreAnimal, opt => opt.Ignore());

        CreateMap<CategoriaProveedor, CategoriaProveedorDTO>()
             .ForMember(dest => dest.Proveedores, opt => opt.MapFrom(src => src.Proveedores != null ? src.Proveedores.Select(p => new ProveedorDTO
             {
                 Id = p.Id,
                 Identificacion = p.Identificacion,
                 Nombre = p.Nombre,
                 Telefono = p.Telefono,
                 Correo = p.Correo,
                 IdCategoriaProveedor = p.IdCategoriaProveedor
             }).ToList() : null))
             .ReverseMap()
             .ForMember(dest => dest.Proveedores, opt => opt.Ignore());

        CreateMap<Proveedor, ProveedorDTO>()
            .ForMember(dest => dest.CategoriaProveedor, opts => opts.MapFrom(src => src.CategoriaProveedor))
            .ReverseMap();

        CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaProducto,
                           opt => opt.MapFrom(src => src.CategoriaProducto != null
                                ? new CategoriaProductoDto { Id = src.CategoriaProducto.Id, Descripcion = src.CategoriaProducto.Descripcion }
                                : null)).ReverseMap()
                                 .ForMember(dest => dest.Proveedor,
                           opt => opt.MapFrom(src => src.Proveedor != null
                                ? new ProveedorDTO { Id = src.Proveedor.Id, Identificacion = src.Proveedor.Identificacion, Nombre = src.Proveedor.Nombre, Estado = src.Estado }
                                : null)).ReverseMap()
                                .ForMember(dest => dest.Identification, opt => opt.Ignore());

        CreateMap<CategoriaProducto, CategoriaProductoDto>()
            .ForMember(dest => dest.Productos,
                       opt => opt.MapFrom(src => src.Productos.Select(p => new ProductoDto { Id = p.Id, Codigo = p.Codigo, Nombre = p.Nombre, Descripcion = p.Descripcion, IdCategoriaProducto = p.IdCategoriaProducto, IdProveedor = p.IdProveedor, Stock = p.Stock, Estado = p.Estado })));

        CreateMap<RegistroVacuna, RegistroVacunaDTO>().ForMember(dest => dest.AnimalObj, opt => opt.MapFrom(src => src.AnimalObj != null ?
           new AnimalDTO { Id = src.AnimalObj.Id, Raza = src.AnimalObj.Raza, Genero = src.AnimalObj.Genero, Codigo = src.AnimalObj.Codigo } : null)).ReverseMap()
            .ForMember(dest => dest.ProductoObj, opt => opt.MapFrom(src => src.ProductoObj != null ?
            new ProductoDto { Id = src.ProductoObj.Id, Codigo = src.ProductoObj.Codigo, Descripcion = src.ProductoObj.Descripcion, Nombre = src.ProductoObj.Codigo } : null)).ReverseMap()
            ;

        CreateMap<Provincia, ProvinciaDTO>()
           .ForMember(dest => dest.Cantones, opt => opt.MapFrom(src => src.Cantones.Select(c => new CantonDTO { CantonID = c.CantonID, Nombre = c.Nombre, ProvinciaID = c.ProvinciaID })))
           .ReverseMap()
           .ForMember(dest => dest.Cantones, opt => opt.Ignore());

        CreateMap<Canton, CantonDTO>()
           .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia != null ? new ProvinciaDTO { ProvinciaID = src.Provincia.ProvinciaID, Nombre = src.Provincia.Nombre } : null))
           .ForMember(dest => dest.Distritos, opt => opt.MapFrom(src => src.Distritos.Select(d => new DistritoDTO { DistritoID = d.DistritoID, Nombre = d.Nombre, CantonID = d.CantonID })))
           .ReverseMap()
           .ForMember(dest => dest.Provincia, opt => opt.Ignore())
           .ForMember(dest => dest.Distritos, opt => opt.Ignore());

        CreateMap<Distrito, DistritoDTO>()
          .ForMember(dest => dest.Canton, opt => opt.MapFrom(src => src.Canton != null ? new CantonDTO { CantonID = src.Canton.CantonID, Nombre = src.Canton.Nombre, ProvinciaID = src.Canton.ProvinciaID } : null))
          .ReverseMap()
          .ForMember(dest => dest.Canton, opt => opt.Ignore());


        CreateMap<EmpleadoDTO, Empleado>().ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol))
            .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia != null ? new ProvinciaDTO { ProvinciaID = src.Provincia.ProvinciaID, Nombre = src.Provincia.Nombre } : null)).ReverseMap()
            .ForMember(dest => dest.Canton, opt => opt.MapFrom(src => src.Canton))
            .ForMember(dest => dest.Distrito, opt => opt.MapFrom(src => src.Distrito))
            .ReverseMap()
            .ForMember(dest => dest.Rol, opt => opt.Ignore())
            .ForMember(dest => dest.Provincia, opt => opt.Ignore())
            .ForMember(dest => dest.Canton, opt => opt.Ignore())
            .ForMember(dest => dest.Distrito, opt => opt.Ignore());

        CreateMap<Vacacion, VacacionDTO>()
            .ForMember(dest => dest.Empleado, opt => opt.MapFrom(src => src.EmpleadoModel != null ? new EmpleadoDTO { Nombre = src.EmpleadoModel.Nombre, Cedula = src.EmpleadoModel.Cedula } : null)).ReverseMap();
        CreateMap<HistoricoPago, HistoricoPagoDTO>().ReverseMap();
        CreateMap<HistoricoPago, HistoricoPagoDTO>().ForMember(dest => dest.Empleado, opt => opt.MapFrom(src => src.Empleado != null ? new EmpleadoDTO { Nombre = src.Empleado.Nombre, Cedula = src.Empleado.Cedula } : null)).ReverseMap();

        CreateMap<RegistroLiquidacion, RegistroLiquidacionDTO>()
          .ForMember(dest => dest.empleadoObj, opt => opt.MapFrom(src => src.Empleado != null ? new EmpleadoDTO
          {
              Nombre = src.Empleado.Nombre,
              Cedula = src.Empleado.Cedula
          } : null))
          .ReverseMap();


        CreateMap<Compra, CompraDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdTipoProducto, opt => opt.MapFrom(src => src.IdTipoProducto))
            .ForMember(dest => dest.IdProveedor, opt => opt.MapFrom(src => src.IdProveedor))
            .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto))
            .ForMember(dest => dest.IdAnimal, opt => opt.MapFrom(src => src.IdAnimal))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.Importe, opt => opt.MapFrom(src => src.Importe))
            .ForMember(dest => dest.FechaCompra, opt => opt.MapFrom(src => src.FechaCompra))
            .ForMember(dest => dest.Proveedor, opt => opt.MapFrom(src => src.Proveedor != null ? new ProveedorDTO
            {
                Id = src.Proveedor.Id,
                Identificacion = src.Proveedor.Identificacion,
                Nombre = src.Proveedor.Nombre,
                Telefono = src.Proveedor.Telefono,
                Correo = src.Proveedor.Correo,
                IdCategoriaProveedor = src.Proveedor.IdCategoriaProveedor,
                Estado = src.Proveedor.Estado
            } : null))
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto != null ? new ProductoDto
            {
                Id = src.Producto.Id,
                Codigo = src.Producto.Codigo,
                Nombre = src.Producto.Nombre,
                Descripcion = src.Producto.Descripcion,
                IdTipoProducto = src.Producto.IdTipoProducto,
                IdCategoriaProducto = src.Producto.IdCategoriaProducto,
                IdProveedor = src.Producto.IdProveedor,
                Stock = src.Producto.Stock,
                Estado = src.Producto.Estado
            } : null))
            .ForMember(dest => dest.Animal, opt => opt.MapFrom(src => src.Animal != null ? new AnimalDTO
            {
                Id = src.Animal.Id,
                Raza = src.Animal.Raza,
                Genero = src.Animal.Genero,
                Codigo = src.Animal.Codigo,
                PartosRegistrados = src.Animal.PartosRegistrados,
                PesoKG = src.Animal.PesoKG,
                FechaNacimiento = src.Animal.FechaNacimiento,
                FechaUltimaVacuna = src.Animal.FechaUltimaVacuna,
                Estado = src.Animal.Estado
            } : null))
            .ReverseMap()
            .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
            .ForMember(dest => dest.Producto, opt => opt.Ignore())
            .ForMember(dest => dest.Animal, opt => opt.Ignore());


        CreateMap<Venta, VentaDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdAnimal, opt => opt.MapFrom(src => src.IdAnimal))
            .ForMember(dest => dest.CedulaCliente, opt => opt.MapFrom(src => src.CedulaCliente))
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.NombreCliente))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.Importe, opt => opt.MapFrom(src => src.Importe))
            .ForMember(dest => dest.FechaVenta, opt => opt.MapFrom(src => src.FechaVenta))
              .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
            .ReverseMap();

        CreateMap<Reporte, ReporteDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.CodigoCVO, opt => opt.MapFrom(src => src.CodigoCVO))
        .ForMember(dest => dest.CodigoTransporte, opt => opt.MapFrom(src => src.CodigoTransporte))
        .ForMember(dest => dest.Transaccion, opt => opt.MapFrom(src => src.Transaccion)) 
        .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
        .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.NombreCliente))
        .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
        .ForMember(dest => dest.Venta, opt => opt.MapFrom(src => src.VentaModel != null ? new VentaDTO
        {
            Id = src.VentaModel.Id,
            Descripcion = src.VentaModel.Descripcion,
            IdAnimal = src.VentaModel.IdAnimal,
            CedulaCliente = src.VentaModel.CedulaCliente,
            NombreCliente = src.VentaModel.NombreCliente,
            FechaVenta = src.VentaModel.FechaVenta
        } : null))
        .ReverseMap();


        
        CreateMap<ReporteDTO, Reporte>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore());
    }

}

