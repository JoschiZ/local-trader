using LocalTrader.Data.Account;

using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Components.Account;

internal sealed class IdentityUserAccessor(
    UserManager<ApplicationUser> userManager,
    IdentityRedirectManager redirectManager)
{
    public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToInvalidUser(userManager, context);
        }

        return user;
    }
}