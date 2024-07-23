using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IRegistroVacuna
    {
        List<RegistroVacuna> GetList();

        Task<RegistroVacuna> GetRegistroVacunaById(long id);

        Task<RegistroVacuna> PutRegistroVacuna(RegistroVacuna RegistroVacuna);

        Task<RegistroVacuna> DeleteRegistroVacuna(long id);

        Task<RegistroVacuna> PostRegistroVacuna(RegistroVacuna RegistroVacuna);

        Task<List<RegistroVacuna>> GetRegistroVacunaStatus(bool Status);
        List<RegistroVacuna> GetListByIdAnimal(long IdAnimal);

    }
}
