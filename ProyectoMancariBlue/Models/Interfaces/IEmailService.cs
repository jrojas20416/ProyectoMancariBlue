using ProyectoMancariBlue.Models.Obj;

namespace ProyectoMancariBlue.Models.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Correo request);
    }
}
