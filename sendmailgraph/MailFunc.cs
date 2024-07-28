using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace appmailteste
{
    public static class MailFunc
    {
        [FunctionName("SendMailFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (req.HasFormContentType)
            {
                var form = await req.ReadFormAsync();
                var files = form.Files.GetFiles("files");
                var fileAttachements = new List<(string FileName, string ContentType, byte[] ContentBytes)>();

                foreach (var file in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();

                        fileAttachements.Add((file.FileName, file.ContentType, fileBytes));
                    }
                }

                string subject = form["subject"];
                string content = form["content"];

                await new MailService().SendMailAsync(subject, content, fileAttachements);

                return new OkObjectResult("E-mail enviado com sucesso");
            }

            return new BadRequestObjectResult(null);
        }
    }
}
