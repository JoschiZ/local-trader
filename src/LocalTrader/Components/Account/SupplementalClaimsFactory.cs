using System.Security.Claims;
using LocalTrader.Data.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LocalTrader.Components.Account;

public class SupplementalClaimsFactory : UserClaimsPrincipalFactory<User>
{
    public SupplementalClaimsFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var oldNameClaim = identity.FindFirst(ClaimTypes.Name);
        if(identity.TryRemoveClaim(oldNameClaim))
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName));
        }
        return identity;
    }
}