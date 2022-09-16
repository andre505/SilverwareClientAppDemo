using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Application.Interfaces
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        public UserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public string GetUserId()
        {
            var claims = _accessor.HttpContext.User.Identities.First().Claims.ToList();

            string userId = claims?.FirstOrDefault(x => x.Type.Equals("uid", StringComparison.OrdinalIgnoreCase))?.Value.ToString();

            return userId;
        }
    }
}
