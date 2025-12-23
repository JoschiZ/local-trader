using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.ClientGen.Kiota;
using FastEndpoints.Swagger;
using Kiota.Builder;
using LocalTrader.Api.Account;
using LocalTrader.Api.Cards.Magic;
using LocalTrader.Components;
using LocalTrader.Components.Account;
using LocalTrader.Data;
using LocalTrader.Data.Account;
using LocalTrader.ServiceDefaults;
using LocalTrader.Shared.Aspire;
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

        x.SerializerSettings = settings =>
        {
            settings.Converters.Add(new JsonStringEnumConverter());
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

        x.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    })
    .UseSwaggerGen(x => { x.Path = "openapi/{documentName}.json"; });
await app.GenerateApiClientsAndExitAsync(x =>
{
    var outputPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.FullName 
                 ?? throw new InvalidOperationException(), "LocalTrader.Client", "ApiClients");
    x.SwaggerDocumentName = "v1";
    x.Language = GenerationLanguage.CSharp;
    x.CleanOutput = true;
    x.ClientClassName = "TraderClient";
    x.ClientNamespaceName = "LocalTrader.Client";
    x.OutputPath = outputPath;
    x.ExcludeBackwardCompatible = true;
}).ConfigureAwait(false);

app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TraderContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}

app.Run();