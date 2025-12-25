using FastEndpoints;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Api.Account.Manage;

public class PasskeyRequestOptionsEndpoint : Endpoint<PasskeyRequestOptionsEndpoint.Request, ContentHttpResult>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAntiforgery _antiforgery;
    public PasskeyRequestOptionsEndpoint(UserManager<User> userManager, SignInManager<User> signInManager, IAntiforgery antiforgery)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _antiforgery = antiforgery;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.PasskeyRequestOptions);
        AllowAnonymous();
    }

    public override async Task<ContentHttpResult> ExecuteAsync(Request req, CancellationToken ct)
    {
        await _antiforgery.ValidateRequestAsync(HttpContext).ConfigureAwait(false);

        var user = string.IsNullOrEmpty(req.Username) ? null 
            : await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);
        
        var optionsJson = await _signInManager.MakePasskeyRequestOptionsAsync(user).ConfigureAwait(false);
        return TypedResults.Content(optionsJson, contentType: "application/json");
    }


    public sealed class Request
    {
        [BindFrom("username")]
        public required string Username { get; set; }
    }
}