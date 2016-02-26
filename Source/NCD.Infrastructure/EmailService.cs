using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NCD.Application.Domain;
using NCD.Application.Services;
using RazorEngine.Templating;

namespace NCD.Infrastructure
{
    public class EmailService : IEmailService
    {
        public void Send(string emailAddress, IEnumerable<Person> persons)
        { 
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                if (persons != null && persons.Any())
                {
                    var pdfs = new List<ReportFile>();

                    foreach (var person in persons)
                    {
                        var htmlTemplate = GenerateHtmlString(person);
                        var pdf = ConvertToPdf(htmlTemplate);
                        pdfs.Add(new ReportFile 
                        {
                            Name = person.Name,
                            Data = pdf
                        });
                    }
                    SendEmail(emailAddress, pdfs);
                }
            }            
        }

        private static void SendEmail(string emailAddress, List<ReportFile> pdfs)
        {
            var client = new SmtpClient();
            var batches = (pdfs.Count / 10) + 1;

            for (var i = 0; i < batches - 1; i++)
            {
                var subject = "Criminal Profiles";

                if (batches > 1)
                {
                    subject += string.Format(" - Part {0}/{1}", (i + 1), batches);
                }

                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(emailAddress));
                    message.From = new MailAddress(@"test.project@crossover.com");
                    message.Subject = subject;
                    message.Body = "Hi, we are sending you the results of your search. Please open the attached files.";
                    message.IsBodyHtml = false;

                    foreach (var pdf in pdfs.Skip(i * 10).Take(10))
                    {
                        MemoryStream stream = new MemoryStream(pdf.Data);
                        Attachment attachment = new Attachment(stream, pdf.Name, "application/pdf");
                        message.Attachments.Add(attachment);
                    }

                    client.Send(message);
                }
            }
        }

        private static string GenerateHtmlString(Person person)
        {
            if (person != null)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Services\EmailTemplate.cshtml");
                var templateService = new TemplateService();
                var template = File.ReadAllText(path);
                var htmlString = templateService.Parse(template, person, null, person.Id.ToString());

                return htmlString;
            }

            return null;
        }

        private static byte[] ConvertToPdf(string htmlTemplate)
        {
            try
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    using (var doc = new Document())
                    {
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            doc.Open();
                            using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                            {
                                using (var sr = new StringReader(htmlTemplate))
                                {
                                    htmlWorker.Parse(sr);
                                }
                            }
                            doc.Close();
                        }
                    }
                    bytes = ms.ToArray();
                }
                return bytes;
            }
            catch { }

            return null;
        }        
    }
}
