using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class RegistroVacunaModel : IRegistroVacuna
    {
        private readonly MancariBlueContext _context;
        public RegistroVacunaModel(MancariBlueContext context)
        {

            _context = context;
        }
        public async Task<RegistroVacuna> DeleteRegistroVacuna(long id)
        {
          var Registro= _context.RegistroVacuna.Find(id);
            _context.RegistroVacuna.Remove(Registro);
           await  _context.SaveChangesAsync();
            return Registro;
        }

        public List<RegistroVacuna> GetList()
        {
            return _context.RegistroVacuna.ToList();
        }
        public List<RegistroVacuna> GetListByIdAnimal(long IdAnimal)
        {
            return _context.RegistroVacuna.Where(x=>x.IdAnimal==IdAnimal).ToList();
        }

        public async Task<RegistroVacuna> GetRegistroVacunaById(long id)
        {

            return await _context.RegistroVacuna.FindAsync(id);
        }

        public Task<List<RegistroVacuna>> GetRegistroVacunaStatus(bool Status)
        {
            throw new NotImplementedException();
        }

        public async Task<RegistroVacuna> PostRegistroVacuna(RegistroVacuna RegistroVacuna)
        {
            await _context.RegistroVacuna.AddAsync(RegistroVacuna);
            await _context.SaveChangesAsync();
            return RegistroVacuna;
        }

        public async Task<RegistroVacuna> PutRegistroVacuna(RegistroVacuna RegistroVacuna)
        {
             _context.RegistroVacuna.Update(RegistroVacuna);
            await _context.SaveChangesAsync();
            return RegistroVacuna;
        }
    }
}
