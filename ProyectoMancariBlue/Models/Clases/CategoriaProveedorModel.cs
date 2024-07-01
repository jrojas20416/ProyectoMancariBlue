using ProyectoMancariBlue.Models.Interfaces;

namespace ProyectoMancariBlue.Models.Clases
{
    public class CategoriaProveedorModel : ICategoriaProveedor
    {
        private readonly MancariBlueContext _context;
        public CategoriaProveedorModel(MancariBlueContext context)
        {

            _context = context;
        }
        public IEnumerable<Obj.CategoriaProveedor> GetAll()
        {
            return _context.CategoriaProveedor.ToList();
        }

        public Obj.CategoriaProveedor GetById(int id)
        {
           return _context.CategoriaProveedor.Find(id);
        }
    }
}
