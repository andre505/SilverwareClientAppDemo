namespace Application.DTOs.Account
{
    public class StaffRegistrationResponse
    {
        public bool UserAlreadyExists { get; set; }
        public string Message { get; set; }
        public string VerificationUrl { get; set; }
    }
}
