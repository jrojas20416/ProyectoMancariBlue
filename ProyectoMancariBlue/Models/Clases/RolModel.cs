using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Clases
{
    public class RolModel : IRolModel
    {

        private readonly MancariBlueContext _context;
        public RolModel(MancariBlueContext context)
        {

            _context = context;
        }

        public async Task<List<Rol>> GetRolAsync()
        {
            return await _context.Rol.ToListAsync();

        }
    }
}
