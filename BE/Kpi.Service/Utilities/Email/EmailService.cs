using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.Utilities.Email
{
    public class EmailModel
    {
        public IEnumerable<string> ReceiversEmail { get; set; }
        public string Subject { get; set; }
        public string[] ContentParams { get; set; }
        public IEnumerable<string> AttachedFilePaths { get; set; }
    }

    public interface IEmailService<T> where T : EmailModel
    {
        void SendNotificationMail(T input);
    }

    public interface IClientEmailService : IEmailService<EmailModel>
    {
    }

    public class EmailService : IClientEmailService, IDisposable
    {
        private static readonly string _host = "";
        private static readonly int _port = 0;
        private static readonly bool _ssl = false;
        private SmtpClient _client;

        public EmailService()
        {
            _client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _ssl,
                Host = _host,
                Port = _port
            };

            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("vbdh@brggroup.vn", "Brg123456");
            _client.UseDefaultCredentials = false;
            _client.Credentials = credentials;
        }

        public void SendNotificationMail(EmailModel emailInfo)
        {
            try
            {
                using (MailMessage msg = new MailMessage
                {
                    From = new MailAddress("vbdh@brggroup.vn")
                })
                {

                    // valid mail
                    var mailValid = new EmailAddressAttribute();

                    emailInfo.ReceiversEmail = emailInfo.ReceiversEmail.Distinct();
                    foreach (var im in emailInfo.ReceiversEmail)
                    {
                        if (mailValid.IsValid(im))
                        {
                            msg.To.Add(new MailAddress(im));
                        }
                    }

                    msg.Subject = emailInfo.Subject;
                    msg.IsBodyHtml = true;

                    string template = this.GetEmailTemplate();
                    msg.Body = string.Format(template, emailInfo.ContentParams);

                    if (emailInfo.AttachedFilePaths != null)
                    {
                        foreach (var path in emailInfo.AttachedFilePaths)
                        {
                            var attachment = new System.Net.Mail.Attachment(path);
                            msg.Attachments.Add(attachment);
                        }
                    }

                    _client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.InnerException.InnerException.Message);
            }
        }
        
        private string GetEmailTemplate()
        {
            return string.Empty;
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}
