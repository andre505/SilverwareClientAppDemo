namespace Application.DTOs.Account
{
    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }
}
