using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface ICategoriaProveedor
    {
        IEnumerable<CategoriaProveedor> GetAll();
        CategoriaProveedor GetById(int id);
       
    }
}
