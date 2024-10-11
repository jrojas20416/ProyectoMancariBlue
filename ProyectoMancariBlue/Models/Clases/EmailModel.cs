
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;
using MailKit.Net.Smtp;
using MailKit.Security;
namespace ProyectoMancariBlue.Models.Clases
{
    public class EmailModel : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailModel(IConfiguration config)
        {
            _config = config;
        }


        public void SendEmail(Correo request)
        {
            try
            {
                var email = new MimeMessage();


                email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:Username"]));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error enviando correo: {ex.Message}");
                throw;
            }

        }
    }
}
