
using MimeKit;
using MimeKit.Text;
using ProyectoMancariBlue.Models.Interfaces;
using ProyectoMancariBlue.Models.Obj;

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

            var email = new MimeMessage();


            email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:Username"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
