using Data.Repositories.Interfaces;
using Entities.Entities;
using Microsoft.AspNet.Identity;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class EmailRepository : IIdentityMessageService, IEmailRepository
    {

        private RestClient GetRestClient()
        {
            var client = new RestClient()
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", ConfigurationManager.AppSettings[Cross.Constants.MailgunApiKey])
            };
            return client;
        }

        private RestRequest GetRestRequest()
        {
            RestRequest request = new RestRequest();
            request.AddParameter("domain", ConfigurationManager.AppSettings[Cross.Constants.MailgunDomain], ParameterType.UrlSegment);
            request.Method = Method.POST;
            return request;
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            EmailMessage emailMessage = new EmailMessage(null)
            {
                To = message.Destination,
                Subject = message.Subject,
                HtmlBody = message.Body
            };

            await SendBySmtpAsync(emailMessage);
        }

        public async Task SendByRestApiAsync(EmailMessage emailMessage)
        {
            RestRequest request = GetRestRequest();
            request.Resource = "{domain}/messages";
            request.AddParameter("from", emailMessage.From);
            if (!string.IsNullOrWhiteSpace(emailMessage.To) && TestEmail.IsEmail(emailMessage.To)) request.AddParameter("to", emailMessage.To);
            if (!string.IsNullOrWhiteSpace(emailMessage.Cc) && TestEmail.IsEmail(emailMessage.Cc)) request.AddParameter("cc", emailMessage.Cc);
            if (!string.IsNullOrWhiteSpace(emailMessage.Bcc) && TestEmail.IsEmail(emailMessage.Bcc)) request.AddParameter("bcc", emailMessage.Bcc);
            if (!string.IsNullOrWhiteSpace(emailMessage.Subject)) request.AddParameter("subject", emailMessage.Subject);
            if (!string.IsNullOrWhiteSpace(emailMessage.HtmlBody)) request.AddParameter("html", emailMessage.HtmlBody);
            if (emailMessage.Attachements.Count > 0)
            {
                foreach (var attachement in emailMessage.Attachements)
                {
                    request.AddFile("attachment", attachement.Content, attachement.ContentName);
                }
            }

            await GetRestClient().ExecuteTaskAsync(request);
        }

        public async Task SendBySmtpAsync(EmailMessage emailMessage)
        {
            // Compose a message
            MimeMessage mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(emailMessage.FromName, emailMessage.From));
            mail.To.Add(new MailboxAddress(emailMessage.ToName, emailMessage.To));
            mail.Subject = emailMessage.Subject;
            mail.Body = new TextPart("html")
            {
                Text = emailMessage.HtmlBody
            };

            // Send it!
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // XXX - Should this be a little different?
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                var a = ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpUser];
                var b = ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpPassword];

                client.Connect(ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpClient], int.Parse(ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpPort]), false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpUser], ConfigurationManager.AppSettings[Cross.Constants.MailgunSmtpPassword]);

                await client.SendAsync(mail);
                client.Disconnect(true);
            }
        }
    }

    /// <summary>
    /// Author:https://www.codeproject.com/Articles/22777/Email-Address-Validation-Using-Regular-Expression
    /// </summary>
    static class TestEmail
    {
        /// <summary>
        /// Regular expression, which is used to validate an E-Mail address.
        /// </summary>
        const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
            @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
            @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
            @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        /// <summary>
        /// Checks whether the given Email-Parameter is a valid E-Mail address.
        /// </summary>
        /// <param name="email">Parameter-string that contains an E-Mail address.</param>
        /// <returns>True, when Parameter-string is not null and 
        /// contains a valid E-Mail address;
        /// otherwise false.</returns>
        public static bool IsEmail(string email)
        {
            bool validated = false;
            if (!string.IsNullOrWhiteSpace(email))
            {

                string[] emails = email.Split(Cross.Constants.EmailRecipientsSeparator);
                if (emails.Length > 0)
                {
                    foreach (var item in emails)
                    {
                        validated = Regex.IsMatch(item, MatchEmailPattern);
                    }
                }
                else
                {
                    validated = Regex.IsMatch(email, MatchEmailPattern);
                }
                return validated;
            }
            else return false;
        }
    }
}