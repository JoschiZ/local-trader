using System.Threading.Tasks;
using LocalTrader.Data.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Components.Account;

internal sealed class IdentityUserAccessor(
    UserManager<User> userManager,
    IdentityRedirectManager redirectManager)
{
    public async Task<User> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager
            .GetUserAsync(context.User)
            .ConfigureAwait(false);

        if (user is null)
        {
            redirectManager.RedirectToInvalidUser(userManager, context);
        }

        return user;
    }
}