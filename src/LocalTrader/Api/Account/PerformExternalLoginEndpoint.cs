using FastEndpoints;
using LocalTrader.Components.Account.Pages.NotInteractive;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace LocalTrader.Api.Account;
public class PerformExternalLoginEndpoint : Endpoint<PerformExternalLoginEndpoint.Request, ChallengeHttpResult>
{
    
    private readonly SignInManager<User> _signInManager;

    public PerformExternalLoginEndpoint(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.ExternalLogin);
        AllowAnonymous();
        AllowFormData(true);
    }

    public override Task<ChallengeHttpResult> ExecuteAsync(Request req, CancellationToken ct)
    {
        IEnumerable<KeyValuePair<string, StringValues>> query =
        [
            new("ReturnUrl", req.ReturnUrl),
            new("Action", ExternalLogin.LoginCallbackAction)
        ];

        var redirectUrl = UriHelper.BuildRelative(
            HttpContext.Request.PathBase,
            "/Account/ExternalLogin",
            QueryString.Create(query));

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(req.Provider, redirectUrl);
        return Task.FromResult(TypedResults.Challenge(properties, [req.Provider]));
    }

    public class Request
    {
        [BindFrom("provider"), FormField]   
        public required string Provider { get; set; }
        
        [BindFrom("returnUrl"), FormField]
        public required string ReturnUrl { get; set; }
    }
}