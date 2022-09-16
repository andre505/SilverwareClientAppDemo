namespace Domain.Settings
{
    public class MailSettings
    {
        public string EmailFrom { get; set; }
        public string SenderDisplayName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DisplayName { get; set; }
        public string MailgunAPIKey { get; set; }
        public string MailgunBaseUri { get; set; }
        public string MailgunEmailDomain { get; set; }
    }
}
