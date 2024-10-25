using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IEmpleadoModel
    {

        Task<Empleado> LoginAsync(EmpleadoLogin empleado);

        Task<Empleado> RContrasenaAsync(RestorePassword empleado);

        Task<bool> CambiarContrasena(CambiarContrasena empleado);

        Task<Empleado> GetEmpleadoById(long id);

        Task<List<Empleado>> GetAllEmpleadosEstado(long idR);

        Task<Empleado> CreateEmpleado(Empleado empleado);

        Task<Empleado> DeleteEmpleadoID(long id);

        Task<Empleado> UpdateEmpleado(Empleado useempleador);
        Task<Empleado> UpdateAdmin(Empleado useempleador);

        public  string HashPassword(string password);

        Task<bool> EmpleadoExists(string cedula, string correo);

        Task<List<Empleado>> GetEmpleadoRol();
        Task<List<Empleado>> GetEmpleados();

        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado> GetByIdAsync(long id);
        Task<Empleado> AddAsync(Empleado empleado);
        Task<Empleado> UpdateAsync(Empleado empleado);
        Task<bool> DeleteAsync(long id);
        Task<Empleado> GetByCedulaAsync(string cedula);
        Task<Empleado> GetByCorreoAsync(string correo);
        Task<Empleado> GetByLoginAsync(string cedula, string contrasena);
        Task<Empleado> UpdateAsyncRole(Empleado empleado);
        Task updateLoginattemptsUser(string IdUser);
        Task<Empleado> GetByCedulaOrEmail(string IdUser);
    }
}
