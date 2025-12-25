using FastEndpoints;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Api.Account;

public class LogoutEndpoint : Endpoint<LogoutEndpoint.Request, RedirectHttpResult>
{
    private readonly SignInManager<User> _signInManager;
    public LogoutEndpoint(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.Logout);
        AllowFormData(true);
        AllowAnonymous();
    }

    public override async Task<RedirectHttpResult> ExecuteAsync(Request req, CancellationToken ct)
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        return TypedResults.LocalRedirect($"~/{req.ReturnUrl}");
    }

    public sealed class Request
    {
        
        [BindFrom("returnUrl"), FormField]
        public required string ReturnUrl { get; set; }
    }
}