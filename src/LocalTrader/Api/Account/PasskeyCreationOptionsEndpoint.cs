using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace LocalTrader.Api.Account;

public class PasskeyCreationOptionsEndpoint : EndpointWithoutRequest<Results<ContentHttpResult, NotFound<string>>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAntiforgery _antiforgery;

    public PasskeyCreationOptionsEndpoint(UserManager<User> userManager, SignInManager<User> signInManager, IAntiforgery antiforgery)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _antiforgery = antiforgery;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.GetPasskeyCreationOptions);
        AllowAnonymous();
    }

    public override async Task<Results<ContentHttpResult, NotFound<string>>> ExecuteAsync(CancellationToken ct)
    {
        await _antiforgery.ValidateRequestAsync(HttpContext).ConfigureAwait(false);

        var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
        if (user is null)
        {
            return TypedResults.NotFound($"Unable to load user with ID '{_userManager.GetUserId(HttpContext.User)}'.");
        }

        var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
        var userName = await _userManager.GetUserNameAsync(user).ConfigureAwait(false) ?? "User";
        var optionsJson = await _signInManager.MakePasskeyCreationOptionsAsync(new()
        {
            Id = userId,
            Name = userName,
            DisplayName = userName
        }).ConfigureAwait(false);
        return TypedResults.Content(optionsJson, contentType: "application/json");
    }
}