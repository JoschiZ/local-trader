using System;
using System.IO;
using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.ClientGen;
using LocalTrader.Api.Account.Authentication;
using LocalTrader.Api.Magic;
using LocalTrader.Components;
using LocalTrader.Components.Account;
using LocalTrader.Data;
using LocalTrader.Data.Account;
using LocalTrader.ServiceDefaults;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Aspire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.Generation.Processors.Security;
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
            
            settings.AddSecurity(IdentityConstants.BearerScheme,
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "JWT issued by your Identity provider.",
                    AuthorizationUrl = ApiRoutes.Account.Login
                }); 
            
            settings.AddSecurity(IdentityConstants.ApplicationScheme,
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = ".AspNetCore.Identity.Application",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Browser cookie set after login.",
                    AuthorizationUrl = ApiRoutes.Account.Login
                });
            settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());    
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

//builder
//    .Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultScheme = IdentityConstants.ApplicationScheme;
//        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//    })
//    .AddIdentityCookies();

services
    .AddAuthentication(AuthenticationConstants.CombinedSchema)
    .AddScheme<AuthenticationSchemeOptions, CompositeIdentityHandler>(AuthenticationConstants.CombinedSchema, null, compositeOptions =>
    {
        compositeOptions.ForwardDefault = IdentityConstants.BearerScheme;
        compositeOptions.ForwardAuthenticate = AuthenticationConstants.CombinedSchema;
    })
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();

builder.Services.AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<TraderContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

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

app.UseFastEndpoints(x =>
    {
        x.Endpoints.RoutePrefix = "api";
        x.Endpoints.ShortNames = true;

        x.Errors.UseProblemDetails(config => { config.IndicateErrorCode = true; });

        x.Versioning.Prefix = "v";
        x.Versioning.PrependToRoute = true;
    })
    .UseSwaggerGen(x => { x.Path = "openapi/{documentName}.json"; });

var path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.FullName, "LocalTrader.Client", "ApiClients");  
await app.GenerateClientsAndExitAsync("v1", path, settings =>
{
    settings.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.SystemTextJson;
    settings.CSharpGeneratorSettings.GenerateNullableReferenceTypes = true;
    settings.ClassName = "TraderClient";
    settings.CSharpGeneratorSettings.Namespace = "LocalTrader.Client.ApiClients";
    settings.CSharpGeneratorSettings.UseRequiredKeyword = true;
    settings.CSharpGeneratorSettings.JsonLibraryVersion = 10;
}, null)
    .ConfigureAwait(false);

app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TraderContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}

await app.RunAsync().ConfigureAwait(false);