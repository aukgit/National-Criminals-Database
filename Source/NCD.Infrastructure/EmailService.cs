﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NCD.Application.Domain;
using NCD.Application.Services;
using RazorEngine.Templating;

namespace NCD.Infrastructure {
    public class EmailService : IEmailService {
        public void Send(string emailAddress, IList<Person> persons) {
            if (!string.IsNullOrWhiteSpace(emailAddress)) {
                if (persons != null && persons.Count > 0) {
                    var pdfs = new List<ReportFile>();
                    foreach (var person in persons) {
                        var htmlTemplate = GenerateHtmlString(person);
                        var pdf = ConvertToPdf(htmlTemplate);
                        pdfs.Add(new ReportFile {
                            Name = person.Name,
                            Data = pdf
                        });
                    }
                    SendEmail(emailAddress, pdfs);
                    pdfs = null;
                    GC.Collect();
                }
            }
        }

        private static void SendEmail(string emailAddress, List<ReportFile> pdfs) {

            var smtpClient = SmtpClientExtend.GetSmtpClient();
            const int take = 10;

            var batches = (pdfs.Count / take) + 1;
            var createdDateTime = DateTime.UtcNow;
            const string pdfType = "application/pdf";

            for (var i = 0; i < batches; i++) {
                var subject = "[National Criminals Database] Criminal Profiles";

                if (batches > 1) {
                    subject += string.Format(" - Part {0}/{1}", (i + 1), batches);
                }
                string body = "Hi, we are sending you the results of your search. Please open the attached files.";

                //using (var message = new MailMessage()) {
                //    message.To.Add(new MailAddress(emailAddress));
                //    //message.From = new MailAddress(@"test.project@crossover.com");
                //    message.From = new MailAddress(smtpClient.SenderEmailAddress);
                //    message.Subject = subject;
                //    message.Body = ;
                //    message.IsBodyHtml = false;

                var batchPdfs = pdfs.Skip(i * 10).Take(take);
                var attachments = new List<Attachment>(take);
                foreach (var pdf in batchPdfs) {
                    var stream = new MemoryStream(pdf.Data);
                    var attachment = new Attachment(stream, pdf.Name + ".pdf", pdfType);
                    attachment.ContentDisposition.CreationDate = createdDateTime;
                    attachment.ContentDisposition.ModificationDate = createdDateTime;
                    attachments.Add(attachment);
                }
                if (attachments.Count > 0) {
                    smtpClient.QuickSend(emailAddress,
                                         subject,
                                         body,
                                         isAsync: false,
                                         attachments: attachments);
                }

            }
        }

        private static string GenerateHtmlString(Person person) {
            if (person != null) {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Services\EmailTemplate.cshtml");
                var templateService = new TemplateService();
                var template = File.ReadAllText(path);
                var htmlString = templateService.Parse(template, person, null, person.Id.ToString());

                return htmlString;
            }

            return null;
        }

        private static byte[] ConvertToPdf(string htmlTemplate) {
            try {
                byte[] bytes;
                using (var ms = new MemoryStream()) {
                    using (var doc = new Document()) {
                        using (var writer = PdfWriter.GetInstance(doc, ms)) {
                            doc.Open();
                            using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc)) {
                                using (var sr = new StringReader(htmlTemplate)) {
                                    htmlWorker.Parse(sr);
                                }
                            }
                            doc.Close();
                        }
                    }
                    bytes = ms.ToArray();
                }
                return bytes;
            } catch {
            }

            return null;
        }
    }
}