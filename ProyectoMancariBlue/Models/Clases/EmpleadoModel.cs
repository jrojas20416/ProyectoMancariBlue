using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using System.Text;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ProyectoMancariBlue.Models.Clases
{
    public class EmpleadoModel : IEmpleadoModel
    {

        private readonly MancariBlueContext _context;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EmpleadoModel(MancariBlueContext context, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Empleado> LoginAsync(EmpleadoLogin empleado)
        {
            try
            {

                var respuesta = _context.Empleado.FirstOrDefault(u => (u.Correo == empleado.Empleado || u.Cedula == empleado.Empleado) && u.Contrasena == empleado.Contrasena);

                if (respuesta != null)
                {
                    respuesta.Rol = _context.Rol.Find(respuesta.IdRol);
                    return respuesta;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                throw new Exception("Ocurrió un error durante el inicio de sesión.", ex);
            }
        }

        public Task<Empleado> RContrasenaAsync(RestorePassword empleado)
        {
            var respuesta = _context.Empleado.FirstOrDefault(u => u.Correo == empleado.Correo && u.Cedula == empleado.Cedula);

            if (respuesta != null)
            {
                var correo = new Correo();
                String C = GeneratePassword();
                correo.To = respuesta.Correo;
                correo.Subject = "Se ha restablecido su contraseña";
                var body = @"
                    <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f2f2f2;
                                padding: 20px;
                            }
                            .container {
                                max-width: 500px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                border-radius: 10px;
                                padding: 40px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            }
                            h1 {
                                color: #212529;
                                font-size: 24px;
                                text-align: center;
                                margin-top: 0;
                            }
                            p {
                                font-size: 16px;
                                color: #333333;
                                margin-top: 20px;
                            }
                            .password {
                                font-size: 32px;
                                color: #212529;
                                text-align: center;
                                margin-top: 20px;
                            }
                
                            
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <h1>Restablecimiento de contraseña</h1>
                            <p>Estimado/a " + respuesta.Nombre + @",</p>
                            <p>Su contraseña ha sido restablecida exitosamente. A continuación, encontrará los detalles de su nueva contraseña:</p>
                            <p class=""password"">" + C + @"</p>              
                        </div>
                    </body>
                    </html>";
                String Ncontrseña = HashPassword(C);
                correo.Body = body;
                _emailService.SendEmail(correo);
                respuesta.Contrasena = Ncontrseña;
                respuesta.RContrasena = true;
                respuesta.Locked = false;
                respuesta.LoginAttempts = 0;
                _context.SaveChanges();

                return Task.FromResult(respuesta);

            }
            else
            {
                return null;
            }


        }

        private static readonly Random random = new Random();
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GeneratePassword()
        {
            var password = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                var character = Characters[random.Next(Characters.Length)];
                password.Append(character);
            }

            return password.ToString();
        }

        public string HashPassword(string password)
        {
            byte[] fixedSalt = new byte[128 / 8];
            //Se utiliza un valor fijo temporalmente
            var salt = Encoding.UTF8.GetBytes("1234567890abcdef");

            // Generar el hash de la contraseña utilizando PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashed;
        }

        public Task<bool> CambiarContrasena(CambiarContrasena empleado)
        {
            bool respuesta = false;
            var id = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
            var contrasena = HashPassword(empleado.oldPassword);

            var usuario = _context.Empleado.FirstOrDefault(u => u.Id == id && u.Contrasena == contrasena);

            if (usuario != null)
            {
                string nCantrasena = HashPassword(empleado.password);
                usuario.Contrasena = nCantrasena;
                usuario.RContrasena = false;
                _context.SaveChanges();
                respuesta = true;

            }
            return Task.FromResult(respuesta);
        }

        public async Task<Empleado> GetEmpleadoById(long id)
        {
            var usuario = await _context.Empleado
                   .Include(u => u.Rol)
                   .FirstOrDefaultAsync(u => u.Id == id);

            return usuario;
        }



        public Task<List<Empleado>> GetAllEmpleadosEstado(long idR)
        {
            var lista = (from u in _context.Empleado
                         join r in _context.Rol on u.IdRol equals r.Id
                         where u.IdRol == idR
                         select new Empleado
                         {
                             Id = u.Id,
                             Cedula = u.Cedula,
                             Nombre = u.Nombre,
                             Correo = u.Correo,
                             Edad = u.Edad,
                             Telefono = u.Telefono,
                             Provincia = u.Provincia,
                             Canton = u.Canton,
                             Distrito = u.Distrito,
                             Puesto = u.Puesto,
                             FechaIngreso = u.FechaIngreso,
                             Salario = u.Salario,
                             Estado = u.Estado,
                             Rol = r,
                         }).ToList();

            if (lista.Count != 0)
            {
                return Task.FromResult(lista);
            }

            return Task.FromResult(new List<Empleado>());
        }

        public async Task<Empleado> CreateEmpleado(Empleado empleado)
        {
            try
            {
                string password = EmpleadoModel.GeneratePassword();
                // await EnviarContrasenna(empleado, password);
                empleado.Contrasena = HashPassword(password);
                var empl = await _context.Empleado.AddAsync(empleado);
                await _context.SaveChangesAsync();

                if (empl.Entity != null)
                {
                    return empl.Entity;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Empleado> DeleteEmpleadoID(long id)
        {
            var empleado = await _context.Empleado.FindAsync(id);

            if (empleado != null)
            {
                empleado.Rol = await _context.Rol.FindAsync(empleado.IdRol);

                if (empleado.Rol.Nombre == "Admin")
                {
                    _context.Empleado.Remove(empleado);
                }
                else
                {
                    empleado.Estado = !empleado.Estado; // Alternar el estado

                    _context.Empleado.Update(empleado);
                }

                await _context.SaveChangesAsync();
                return empleado;
            }

            return null;
        }

        public async Task<Empleado> UpdateEmpleado(Empleado empleado)
        {
            var empleadoExistente = await _context.Empleado.FindAsync(empleado.Id);

            if (empleadoExistente != null)
            {
                if (empleado.Nombre != null)
                {
                    empleadoExistente.Nombre = empleado.Nombre;
                }

                if (!string.IsNullOrEmpty(empleado.Correo))
                {
                    empleadoExistente.Correo = empleado.Correo;
                }
                if (empleado.Edad != null)
                {
                    empleadoExistente.Edad = empleado.Edad;
                }
                if (empleado.Telefono != null)
                {
                    empleadoExistente.Telefono = empleado.Telefono;
                }
                if (empleado.Provincia != null)
                {
                    empleadoExistente.Provincia = empleado.Provincia;
                }
                if (empleado.Canton != null)
                {
                    empleadoExistente.Canton = empleado.Canton;
                }
                if (empleado.Distrito != null)
                {
                    empleadoExistente.Distrito = empleado.Distrito;
                }
                if (empleado.Puesto != null)
                {
                    empleadoExistente.Puesto = empleado.Puesto;
                }
                if (empleado.FechaIngreso != null)
                {
                    empleadoExistente.FechaIngreso = empleado.FechaIngreso;
                }
                if (empleado.Salario != null)
                {
                    empleadoExistente.Salario = empleado.Salario;
                }
                if (empleado.Estado != null)
                {
                    empleadoExistente.Estado = empleado.Estado;
                }
                if (empleado.IdRol != null)
                {
                    empleadoExistente.IdRol = empleado.IdRol;
                }
                await _context.SaveChangesAsync(); // Utiliza SaveChangesAsync en lugar de SaveChanges
                return empleadoExistente;
            }

            return null;
        }

        public async Task<Empleado> UpdateAdmin(Empleado user)
        {
            var usuarioExistente = await _context.Empleado.FindAsync(user.Id);

            if (usuarioExistente != null)
            {
                if (user.IdRol != null)
                {
                    usuarioExistente.IdRol = user.IdRol;
                }
                if (user.Nombre != null)
                {
                    usuarioExistente.Nombre = user.Nombre;
                }

                if (!string.IsNullOrEmpty(user.Correo))
                {
                    usuarioExistente.Correo = user.Correo;
                }
                await _context.SaveChangesAsync(); // Utiliza SaveChangesAsync en lugar de SaveChanges
                return usuarioExistente;
            }

            return null;
        }



        public async Task<bool> EmpleadoExists(string cedula, string correo)
        {
            return (_context.Empleado?.Any(e => e.Correo == correo || e.Cedula == cedula)).GetValueOrDefault();
        }

        public async Task<List<Empleado>> GetEmpleadoRol()

        {
            return await _context.Empleado.Where(m => m.Id != 1 && m.IdRol != 1).ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleados()

        {

            return await _context.Empleado.
                Include(rol => rol.Rol) // Carga los datos del Usuario relacionado
    .ToListAsync();
        }


        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleado
                .Include(e => e.Provincia)
                .Include(e => e.Canton)
                .Include(e => e.Distrito)
                .Include(e => e.Rol)
                .ToListAsync();
        }

        public async Task<Empleado> GetByIdAsync(long id)
        {
            return await _context.Empleado
                .Include(e => e.Provincia)
                .Include(e => e.Canton)
                .Include(e => e.Distrito)
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Empleado> AddAsync(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado> UpdateAsync(Empleado empleado)
        {
            try
            {

                var existingEntity = _context.Empleado.Local.FirstOrDefault(e => e.Id == empleado.Id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }

                _context.Empleado.Update(empleado);
                await _context.SaveChangesAsync();
                return empleado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Empleado> UpdateAsyncRole(Empleado empleado)
        {
            try
            {
                string password = EmpleadoModel.GeneratePassword();
                await EnviarContrasenna(empleado, password);
                empleado.Contrasena = HashPassword(password);
                empleado.Estado = true;
                var empl = _context.Empleado.Update(empleado);
                await _context.SaveChangesAsync();
                if (empl.Entity != null)
                {
                    return empl.Entity;
                }
                _context.Empleado.Update(empleado);
                await _context.SaveChangesAsync();
                return empleado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteAsync(long id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
            {
                return false;
            }

            _context.Empleado.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Empleado> GetByCedulaAsync(string cedula)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.Cedula == cedula);
        }

        public async Task<Empleado> GetByCorreoAsync(string correo)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.Correo == correo);
        }

        public async Task<Empleado> GetByLoginAsync(string cedula, string contrasena)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.Cedula == cedula && e.Contrasena == contrasena);
        }
        public Task<Empleado> EnviarContrasenna(Empleado empleado)
        {
            var respuesta = _context.Empleado.FirstOrDefault(u => u.Correo == empleado.Correo && u.Cedula == empleado.Cedula);

            if (respuesta != null)
            {
                var correo = new Correo();
                String C = empleado.Contrasena;
                correo.To = respuesta.Correo;
                correo.Subject = "Creación de usuario";
                var body = @"
                    <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f2f2f2;
                                padding: 20px;
                            }
                            .container {
                                max-width: 500px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                border-radius: 10px;
                                padding: 40px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            }
                            h1 {
                                color: #212529;
                                font-size: 24px;
                                text-align: center;
                                margin-top: 0;
                            }
                            p {
                                font-size: 16px;
                                color: #333333;
                                margin-top: 20px;
                            }
                            .password {
                                font-size: 32px;
                                color: #212529;
                                text-align: center;
                                margin-top: 20px;
                            }
                
                            
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <h1>Creación de usuario</h1>
                            <p>Estimado/a " + respuesta.Nombre + @",</p>
                            <p>Se ha creado el usuario. A continuación, encontrará los detalles de su nueva contraseña:</p>
                            <p class=""password"">" + C + @"</p>              
                        </div>
                    </body>
                    </html>";
                String Ncontrseña = HashPassword(C);
                correo.Body = body;
                _emailService.SendEmail(correo);
                respuesta.Contrasena = Ncontrseña;
                respuesta.RContrasena = true;
                _context.SaveChanges();

                return Task.FromResult(respuesta);

            }
            else
            {
                return null;
            }


        }
        public Task<Empleado> EnviarContrasenna(Empleado empleado, string password)
        {


            if (empleado != null)
            {
                var correo = new Correo();
                //String C = empleado.Contrasena;
                correo.To = empleado.Correo;
                correo.Subject = "Creación de usuario";
                var body = @"
                    <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f2f2f2;
                                padding: 20px;
                            }
                            .container {
                                max-width: 500px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                border-radius: 10px;
                                padding: 40px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            }
                            h1 {
                                color: #212529;
                                font-size: 24px;
                                text-align: center;
                                margin-top: 0;
                            }
                            p {
                                font-size: 16px;
                                color: #333333;
                                margin-top: 20px;
                            }
                            .password {
                                font-size: 32px;
                                color: #212529;
                                text-align: center;
                                margin-top: 20px;
                            }
                
                            
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <h1>Creación de usuario</h1>
                            <p>Estimado/a " + empleado.Nombre + @",</p>
                            <p>Se ha creado el usuario. A continuación, encontrará los detalles de su nueva contraseña:</p>
                            <p class=""password"">" + password + @"</p>              
                        </div>
                    </body>
                    </html>";
                string Ncontrseña = password;
                correo.Body = body;
                _emailService.SendEmail(correo);
                empleado.Contrasena = Ncontrseña;
                empleado.RContrasena = true;
                empleado.LoginAttempts = 0;
                empleado.Locked = false;
                _context.SaveChanges();

                return Task.FromResult((Empleado)empleado);

            }
            else
            {
                return null;
            }


        }

        public async Task updateLoginattemptsUser(string IdUser)
        {
            var user = await _context.Empleado
                .FirstOrDefaultAsync(e => e.Correo == IdUser || e.Cedula == IdUser);

            if (user != null)
            {
                if (!user.Locked)
                {
                    user.LoginAttempts = user.LoginAttempts + 1;
                    if (user.LoginAttempts >= 3)
                    {
                        user.Locked = true;
                    }
                }

            }
            _context.SaveChanges();

        }
        public async Task<Empleado> GetByCedulaOrEmail(string IdUser)
        {
            var user = await _context.Empleado
                .FirstOrDefaultAsync(e => e.Correo == IdUser || e.Cedula == IdUser);

            return user;

        }
    }
}


