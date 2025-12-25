using FastEndpoints;
using FastEndpoints.Swagger;
using LocalTrader.Api.Account;
using LocalTrader.Api.Magic;
using LocalTrader.Components;
using LocalTrader.Components.Account;
using LocalTrader.Data;
using LocalTrader.Data.Account;
using LocalTrader.ServiceDefaults;
using LocalTrader.Shared.Aspire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NSwag;
using Scalar.AspNetCore;
using ScryfallApi.Client;
using _Imports = LocalTrader.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;

services.AddIdentityApiEndpoints<User>();

services.AddScoped<IMagicCardRepository, MagicCardRepository>();
services.AddScryfallApiClient();


services.AddFastEndpoints(x => { x.IncludeAbstractValidators = true; })
    .SwaggerDocument(x =>
    {
        x.EnableJWTBearerAuth = false;
        x.DocumentSettings = settings =>
        {
            settings.MarkNonNullablePropsAsRequired();
            settings.AddAuth(IdentityConstants.ApplicationScheme, new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                In = OpenApiSecurityApiKeyLocation.Cookie,
                Name = ".AspNetCore.Identity.Application"
            });
            settings.DocumentName = "v1";
        };
    });

// Add MudBlazor services
builder.Services.AddMudServices();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
s
services
    .AddAuthentication(IdentityConstants.BearerAndApplicationScheme)
    .AddScheme<AuthenticationSchemeOptions, CompositeIdentityHandler>(IdentityConstants.BearerAndApplicationScheme, null, compositeOptions =>
    {
        compositeOptions.ForwardDefault = IdentityConstants.BearerScheme;
        compositeOptions.ForwardAuthenticate = IdentityConstants.BearerAndApplicationScheme;
    })
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, SupplementalClaimsFactory>();

builder.AddNpgsqlDbContext<TraderContext>(Services.LocalTraderDb, x =>
{
    
}, optionsBuilder =>
{
    if (builder.Environment.IsDevelopment())
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
    }
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<TraderContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseFastEndpoints(x =>
    {
        x.Endpoints.RoutePrefix = "api";
        x.Endpoints.ShortNames = true;

        x.Errors.UseProblemDetails(config => { config.IndicateErrorCode = true; });

        x.Versioning.Prefix = "v";
        x.Versioning.PrependToRoute = true;
    })
    .UseSwaggerGen(x => { x.Path = "openapi/{documentName}.json"; });

app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TraderContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}

await app.RunAsync().ConfigureAwait(false);