using Ecom.Core.Dto;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDto emailDto)
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("My Ecom", configuration["EmailSetting:From"]));
            mimeMessage.Subject = emailDto.Subject;
            mimeMessage.To.Add(new MailboxAddress(emailDto.To,emailDto.To));
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDto.Content
            };
            using(var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]), true);
                    await smtp.AuthenticateAsync(configuration["EmailSetting:Username"],
                        configuration["EmailSetting:Password"]);
                    await smtp.SendAsync(mimeMessage);
                }
                catch (Exception ex) 
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
