using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Mailer
{
    public static class Mailer
    {

        public static async Task SendEmail(EmailSettings settings, List<EmailAddress> recipients, object emailDataTemplate, List<Attachment> attachments = null)
        {

            var client = new SendGridClient(settings.Email_SendGridKey);
            var msg = new SendGridMessage();
            var from = new EmailAddress(settings.Email_From, settings.Email_FromName);



            msg.SetFrom(from);
            msg.AddTos(recipients);
            msg.SetTemplateId(settings.Template_Id);


            msg.SetTemplateData(emailDataTemplate);


            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                throw new Exception(String.Format("Could not send email! Response code from SendGrid {0}", response.StatusCode));


        }

    }

    public class MailAttachment
    {
        public string base64string { get; set; }
        public string filename { get; set; }
        public string extension { get; set; }
    }
}
