namespace Domain.Settings
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }

    public class SilverwarePOSEmailSenders
    {
        public string Admin { get; set; }
        public string Operations { get; set; }
        public string Finance { get; set; }
        public string HumanResources { get; set; }
        public string Legal { get; set; }
        public string Engineering { get; set; }
        public string CustomerService { get; set; }
        public string Marketing { get; set; }
        public string ContentDevelopment { get; set; }
        public string ITNetworks { get; set; }
        public string Contact { get; set; }
    }
}
