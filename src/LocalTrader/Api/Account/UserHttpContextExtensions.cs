using System.Security.Claims;
using LocalTrader.Shared.Api.Account.Users;
using Microsoft.Extensions.Options;

namespace LocalTrader.Api.Account;

public static class UserHttpContextExtensions
{
    extension(HttpContext httpContext)
    {
        public UserId? GetUserId()
        {
            if (!httpContext.User.Identity?.IsAuthenticated != true)
            {
                return null;
            }
            
            var claim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                return null;
            }
            
            return UserId.Parse(claim);
        }
    }
}