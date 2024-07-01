using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using ProyectoMancariBlue.Models.Obj.DTO;

namespace ProyectoMancariBlue.Controllers
{

    public class InventarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProducto _producto;
        private readonly ICategoriaProducto _categoriaProducto;
        private readonly IProveedor _proveedor;

        public InventarioController(IMapper mapper, IProducto producto, ICategoriaProducto categoriaProducto, IProveedor proveedor)
        {
            this._mapper = mapper;
            this._producto = producto;
            this._categoriaProducto = categoriaProducto;
            _proveedor = proveedor;
        }
        public async Task<IActionResult> Index()
        {
            var producto = _mapper.Map<List<ProductoDto>>( await _producto.GetAllAsync());
            await FillDropDownListSeachCategoryProduct();
            FillDropDownListSeachProductSupplier();
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
        [HttpPost]
        public IActionResult Create(ProductoDto producto)
        {
            var proveedor = _proveedor.GetByIdentification(producto.Identification);
            producto.IdProveedor = proveedor.Id;
            var ProductoE = _mapper.Map<Producto>(producto);
            var respuesta =  _producto.Add(ProductoE);
            if (respuesta != null)
            {

                return Json("Se creado el registro: " + respuesta.Nombre + " con éxito");
            }
       
            return Json("Error al crear el producto");
        }

        [HttpPost]
        public IActionResult Edit(ProductoDto producto)
        {
            var proveedor = _proveedor.GetByIdentification(producto.Identification);
            producto.IdProveedor = proveedor.Id;
            var ProductoE = _mapper.Map<Producto>(producto);
            var respuesta = _producto.Update(ProductoE);
            if (respuesta != null)
            {

                return Json("Se modificado el registro: " + respuesta.Nombre + " con éxito");
            }
        
            return Json("Error al modificar el producto");
        }
        [HttpPost]
        public async Task<IActionResult> ModifyStock(ProductoDto producto)
        {
            var productoE =await _producto.GetByIdAsync(producto.Id);
            UpdateStock(productoE, producto.Stock);
            
            var respuesta = _producto.Update(productoE);
            if (respuesta != null)
            {

                return Json("Se modificado la cantidad disponible del producto: " + respuesta.Nombre + " con éxito");
            }
         
            return Json("Error al modificar stock");
        }
        public void UpdateStock(Producto producto, int cantidad) {
            producto.Stock += cantidad;
        }


        public IActionResult VerProducto()
        {
            return View();
        }
        public async Task FillDropDownListSeachCategoryProduct()
        {
            var CategoryProduc = await _categoriaProducto.GetAllAsync();

            var categoryProductList = CategoryProduc.Select(categoryL => new SelectListItem
            {
                Value = categoryL.Id.ToString(),
                Text = categoryL.Descripcion,

            });


            ViewBag.categoryProductList = categoryProductList;

        }
        public  void FillDropDownListSeachProductSupplier()
        {
            var ProductSupplier =  _proveedor.GetAllByStatus(true);

            var productSupplierList = ProductSupplier.Select(supplierL => new SelectListItem
            {
                Value = supplierL.Identificacion.ToString(),
                Text = supplierL.Nombre,

            });


            ViewBag.productSupplierList = productSupplierList;

        }
    }
}