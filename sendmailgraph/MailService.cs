using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appmailteste
{
    internal class MailService
    {
        #region Constants
        private const string TenantId = ""; // Your tenant id
        private const string ClientId = ""; // Your client id
        private const string ClientSecret = ""; // Your client secret

        private const string ToEmail = ""; // The email to send the message
        private const string FromEmail = ""; // The email from send the message
        #endregion

        public async Task SendMailAsync(string subject, string content, List<(string FileName, string ContentType, byte[] ContentBytes)> files)
        {
            var credentials = new ClientSecretCredential(TenantId, ClientId, ClientSecret);
            var graphClient = new GraphServiceClient(credentials);
            var attachaments = new List<Attachment>();

            foreach (var file in files)
            {
                attachaments.Add(new FileAttachment
                {
                    OdataType = "#microsoft.graph.fileAttachment",
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    ContentBytes = file.ContentBytes,
                });
            }

            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = content,
                },
                ToRecipients = new List<Recipient>() {
                    new Recipient{
                        EmailAddress = new EmailAddress{
                            Address = ToEmail
                        }
                    }
                },
                Attachments = attachaments
            };

            var body = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = message
            };

            await graphClient.Users[FromEmail].SendMail.PostAsync(body);
        }
    }
}
