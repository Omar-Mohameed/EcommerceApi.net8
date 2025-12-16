using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS.AuthDTOS;
using Test.Core.Services;

namespace Test.Infrastructure.Repositores.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(EmailDto emailDTO)
        {
            // Create content the email message
            MimeMessage message = new MimeMessage(); // type of email message (Subject, Body, From, To)

            message.From.Add(new MailboxAddress("Ecom Team", configuration["EmailSetting:From"]));
            message.Subject = emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart("html")
            {
                Text = emailDTO.Content
            };

            // Create SMTP Client to send the email
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    await smtp.ConnectAsync(configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]),true);

                    // Authenticate with the SMTP server(sign in)
                    await smtp.AuthenticateAsync(configuration["EmailSetting:From"],
                        configuration["EmailSetting:Password"]);
                    // Send the email
                    await smtp.SendAsync(message);

                }
                catch
                {
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
