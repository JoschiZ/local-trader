using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;

namespace LocalTrader.Api.Account;

public class LoginEndpoint : Endpoint<LoginEndpoint.Request, Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
{
    private readonly SignInManager<User> _signInManager;

    public LoginEndpoint(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.Login);
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var useCookieScheme = (req.UseCookies == true) || (req.UseSessionCookies == true);
        var isPersistent = (req.UseCookies == true) && (req.UseSessionCookies != true);
        _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(req.Email, req.Password, isPersistent, lockoutOnFailure: true).ConfigureAwait(false);

        if (result.RequiresTwoFactor)
        {
            if (req is { TwoFactorCode: null, TwoFactorRecoveryCode: null })
            {
                TypedResults.LocalRedirect("~/Account/LoginWith2fa");
            }
            
            if (!string.IsNullOrEmpty(req.TwoFactorCode))
            {
                result = await _signInManager.TwoFactorAuthenticatorSignInAsync(req.TwoFactorCode, isPersistent, rememberClient: isPersistent).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(req.TwoFactorRecoveryCode))
            {
                result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(req.TwoFactorRecoveryCode).ConfigureAwait(false);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
    }
    
    public class Request
    {
        [QueryParam, BindFrom("useCookies"), JsonIgnore]
        public bool? UseCookies { get; init; }
        [QueryParam, BindFrom("useSessionCookies"), JsonIgnore]
        public bool? UseSessionCookies { get; init; }
        
        /// <summary>
        /// The user's email address which acts as a user name.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; init; }

        /// <summary>
        /// The optional two-factor authenticator code. This may be required for users who have enabled two-factor authentication.
        /// This is not required if a <see cref="TwoFactorRecoveryCode"/> is sent.
        /// </summary>
        public string? TwoFactorCode { get; init; }

        /// <summary>
        /// An optional two-factor recovery code from <see cref="TwoFactorResponse.RecoveryCodes"/>.
        /// This is required for users who have enabled two-factor authentication but lost access to their <see cref="TwoFactorCode"/>.
        /// </summary>
        public string? TwoFactorRecoveryCode { get; init; }
    }
}