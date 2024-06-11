using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IRolModel
    {
        Task<List<Rol>> GetRolAsync();
    }
}
