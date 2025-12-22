using FastEndpoints;
using FastEndpoints.Swagger;
using LocalTrader.Api.Account;
using LocalTrader.Api.Cards.Magic;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using LocalTrader.Components;
using LocalTrader.Components.Account;
using LocalTrader.Data;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Aspire;
using Microsoft.EntityFrameworkCore;
using NSwag;
using Scalar.AspNetCore;
using ScryfallApi.Client;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var services = builder.Services;


services.AddScoped<IMagicCardRepository, MagicCardRepository>();
services.AddScryfallApiClient();


services.AddFastEndpoints(x =>
    {
        x.IncludeAbstractValidators = true;
    })
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
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, SupplementalClaimsFactory>();

builder.AddNpgsqlDbContext<TraderContext>(Services.LocalTraderDb);


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
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LocalTrader.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseFastEndpoints(x =>
    {
        x.Endpoints.RoutePrefix = "api";
        x.Endpoints.ShortNames = true;

        x.Errors.UseProblemDetails(config =>
        {
            config.IndicateErrorCode = true;
        });

        x.Versioning.Prefix = "v";
        x.Versioning.PrependToRoute = true;
    })
    .UseSwaggerGen(x =>
    {
        x.Path = "openapi/{documentName}.json";
    });
app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TraderContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}

app.Run();