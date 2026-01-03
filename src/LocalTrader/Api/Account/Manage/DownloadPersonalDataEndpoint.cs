using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LocalTrader.Api.Account.Manage;

public class DownloadPersonalDataEndpoint : EndpointWithoutRequest<Results<NotFound<string>, FileContentHttpResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DownloadPersonalDataEndpoint> _logger;

    public DownloadPersonalDataEndpoint(UserManager<User> userManager, ILogger<DownloadPersonalDataEndpoint> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public override void Configure()
    {
        Post(ApiRoutes.Account.Manage.DownloadPersonalData);
    }

    public override async Task<Results<NotFound<string>, FileContentHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
        if (user is null)
        {
            return TypedResults.NotFound($"Unable to load user with ID '{_userManager.GetUserId(HttpContext.User)}'.");
        }

        var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
        _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);
        
        // Only include personal data for download
        var personalDataProps = typeof(User).GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        var personalData = personalDataProps
            .ToDictionary(
                p => p.Name, 
                p => p.GetValue(user)?.ToString() ?? "null");

        var logins = await _userManager.GetLoginsAsync(user).ConfigureAwait(false);
        foreach (var l in logins)
        {
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        personalData.Add("Authenticator Key", (await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false))!);
        var fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);

        HttpContext.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
        return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");
    }
}