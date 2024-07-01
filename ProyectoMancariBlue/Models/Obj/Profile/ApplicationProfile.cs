using AutoMapper;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Animal, AnimalDTO>()
            .ForMember(dest => dest.PadreAnimal, opt => opt.MapFrom(src => src.PadreAnimal != null ? new AnimalDTO { Id = src.PadreAnimal.Id, Raza = src.PadreAnimal.Raza, Genero = src.PadreAnimal.Genero, Codigo=src.PadreAnimal.Codigo } : null))
            .ForMember(dest => dest.MadreAnimal, opt => opt.MapFrom(src => src.MadreAnimal != null ? new AnimalDTO { Id = src.MadreAnimal.Id, Raza = src.MadreAnimal.Raza, Genero = src.MadreAnimal.Genero, Codigo = src.MadreAnimal.Codigo } : null));

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
                                ? new ProveedorDTO { Id = src.Proveedor.Id, Identificacion=src.Proveedor.Identificacion,Nombre=src.Proveedor.Nombre,Estado=src.Estado }
                                : null)).ReverseMap()
                                .ForMember(dest => dest.Identification, opt => opt.Ignore());

        CreateMap<CategoriaProducto, CategoriaProductoDto>()
            .ForMember(dest => dest.Productos,
                       opt => opt.MapFrom(src => src.Productos.Select(p => new ProductoDto { Id = p.Id, Codigo = p.Codigo, Nombre = p.Nombre, Descripcion = p.Descripcion, IdCategoriaProducto = p.IdCategoriaProducto, IdProveedor = p.IdProveedor, Stock = p.Stock ,Estado=p.Estado})));

    }
}
