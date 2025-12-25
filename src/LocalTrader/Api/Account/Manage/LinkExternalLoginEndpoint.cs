using FastEndpoints;
using LocalTrader.Components.Account.Pages.NotInteractive.Manage;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Api.Account.Manage;

public class LinkExternalLoginEndpoint : Endpoint<LinkExternalLoginEndpoint.Request, ChallengeHttpResult>
{
    private readonly SignInManager<User> _signInManager;

    public LinkExternalLoginEndpoint(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.Manage.LinkExternalLogin);
        AllowFormData(true);
    }

    public override async Task<ChallengeHttpResult> ExecuteAsync(Request req, CancellationToken ct)
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

        var redirectUrl = UriHelper.BuildRelative(
            HttpContext.Request.PathBase,
            "/Account/Manage/ExternalLogins",
            QueryString.Create("Action", ExternalLogins.LinkLoginCallbackAction));

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            req.Provider, 
            redirectUrl,
            _signInManager.UserManager.GetUserId(HttpContext.User));
        return TypedResults.Challenge(properties, [req.Provider]);
    }

    public sealed class Request
    {
        [BindFrom("required"), FormField]
        public required string Provider { get; set; }
    }
}