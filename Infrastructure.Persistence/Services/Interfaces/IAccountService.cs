using Application.DTOs.Account;
using Application.Services.Interfaces;
using Application.Wrappers;
using Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public interface IAccountService 
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<RegisterEmployeeResponse>> RegisterEmployeeAsync(RegisterEmployeeRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string jwtToken);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        bool RevokeToken(string token);
        ApplicationUser GetById(string id);
    }
}
