using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IAnimalModel
    {
        List<Animal> GetAnimal();

        Task<Animal> GetAnimalById(long id);

        Task<Animal> PutAnimal(Animal animal);

        Task<Animal> DeleteAnimal(long id);

        Task<Animal> PostAnimal(Animal animal);

        Task<List<Animal>> GetAnimalStatus(bool Status);
        List<Animal> SearchByGender(string gender);
    }
}
