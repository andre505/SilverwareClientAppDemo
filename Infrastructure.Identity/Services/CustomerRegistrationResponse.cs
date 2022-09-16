namespace Application.Interfaces
{
    public class CustomerRegistrationResponse
    {
        public string Message { get; set; }
        public bool UserAlreadyExists { get; set; }
    }
}