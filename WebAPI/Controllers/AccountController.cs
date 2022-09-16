using Application.DTOs.Account;
using Application.Exceptions;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace SilverwarePOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;
        public AccountController(IAccountService accountService, ILogger logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var result = await _accountService.AuthenticateAsync(request, GenerateIPAddress());
            SetRefreshTokenInCookie(result.Data.RefreshToken);
            return Ok(result);
        }

        [HttpPost("register-employee")]
        public async Task<IActionResult> RegisterAsync(RegisterEmployeeRequest request)
        {

            var origin = Request.Headers["origin"].ToString();
            return Ok(await _accountService.RegisterEmployeeAsync(request, origin));

        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _accountService.ResetPassword(model));
        }

        private string GenerateIPAddress()
        {
            try
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                    return Request.Headers["X-Forwarded-For"];
                else
                    return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message.ToString());
            }
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            // accept token from request body or cookie
            string refreshToken = model.Token ?? Request.Cookies["refreshToken"];
            var response = await _accountService.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.Data.RefreshToken))
                SetRefreshTokenInCookie(response.Data.RefreshToken);
            return Ok(response);

        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {

            string token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _accountService.RevokeToken(token);

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }


        private void SetRefreshTokenInCookie(string refreshToken)
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(10),
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message.ToString());
            }
        }


        [HttpPost("tokens/{id}")]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = _accountService.GetById(id);
            return Ok(user.RefreshToken);
        }
    }
}