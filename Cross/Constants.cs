
namespace Cross
{
    public static class Constants
    {
        #region Infraestructure Strings
        public const string DefaultConnectionString = "DefaultConnection";
        public const string AutofacWebRequest = "AutofacWebRequest";
        public const string DataProtectorTokenProvider = "Uniayuda";
        public const string UserSessionDataKey = "UserSessionData";
        public const string UrlEnvironment= "URL_ENVIRONMENT";
        public const string PayPalEnvironment = "PAYPAL_ENVIRONMENT";

        public const string MailgunDomain = "MAILGUN_DOMAIN";
        public const string MailgunApiKey = "MAILGUN_API_KEY";
        public const string MailgunSmtpUser = "MAILGUN_SMTP_USER";
        public const string MailgunSmtpPassword = "MAILGUN_SMTP_PASSWORD";
        public const string MailgunSmtpClient = "MAILGUN_SMTP_CLIENT";
        public const string MailgunSmtpPort = "MAILGUN_SMTP_PORT";
        public const char EmailRecipientsSeparator = ',';
        public const string SenderEmail = "no-reply@uniayuda.com";
        public const string SenderName = "Uniayuda";
        public const string RecipientName = "User";
        #endregion

        #region DataBase Constants
        public const string CountryNone = "440adcce-99f6-41aa-9fdc-c8c0431cc2f8";
        public const string ProfessionNone = "dbac9962-8337-4b23-8f43-2e2325d50720";
        //public const string PhotoNone = "1bc2df95-9103-491b-af1d-8c55550390b3";
        #endregion

        public const int passwordMinimumLength = 8;
        public const double minimumInvestment = 1;
    }
}
