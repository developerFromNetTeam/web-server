using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using web_server.ibl;

namespace web_server.bl
{
    public class MailClient : IMailClient
    {
        private IConfiguration configuration;

        private SmtpClient smtpClient;

        public MailClient(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.InitializeSmtpClient();
        }

        private void InitializeSmtpClient()
        {
            var smtpServer = this.configuration["smtpServer"];
            var smtpUser = this.configuration["smtpUser"];
            var smtpPass = this.configuration["smtpPass"];
            var fromEmail = this.configuration["fromEmail"];
            var toEmail = this.configuration["toEmail"];
            var smtpPort = this.configuration["smtpPort"];


            if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPass) || string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(smtpPort))
            {
                throw new Exception("Smtp configuration is incorrect.");
            }

            this.smtpClient = new SmtpClient(smtpServer, int.Parse(smtpPort));
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.EnableSsl = true;
            this.smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
        }

        public async Task SendAsync(MailModel model)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.configuration["fromEmail"]);
            mailMessage.To.Add(this.configuration["toEmail"]);
            mailMessage.Body = model.Body;
            mailMessage.Subject = model.Subject;
            await Task.Run(() =>
            {
                this.smtpClient.Send(mailMessage);
            });
        }
    }
}
