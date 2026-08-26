using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Licensing.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using NIHR.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Repositories;
using Nihr.Hub.Infrastructure.Services;
using Nihr.Hub.Infrastructure.Settings;
using Nihr.Hub.Web;
using Nihr.Hub.Web.Content;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.AddNihrConfiguration();
builder.ConfigureNihrLogging();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
}) // Use cookies for session tracking
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ??
                       throw new InvalidOperationException("ClientId is missing");
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ??
                           throw new InvalidOperationException("ClientSecret is missing");

    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };
});

if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("DevelopmentModeAuthentication:Enabled"))
{
    builder.Services.AddAuthentication(nameof(DevelopmentModeAuthenticationHandler))
        .AddScheme<DevelopmentModeAuthenticationOptions, DevelopmentModeAuthenticationHandler>(
            nameof(DevelopmentModeAuthenticationHandler),
            options =>
            {
                options.Name = builder.Configuration.GetValue<string>("DevelopmentModeAuthentication:Name") ?? string.Empty;
                options.GivenName = builder.Configuration.GetValue<string>("DevelopmentModeAuthentication:GivenName") ?? string.Empty;
                options.Email = builder.Configuration.GetValue<string>("DevelopmentModeAuthentication:Email") ?? string.Empty;
            }
        );
}

builder.Services.AddOptions<AupSettings>()
    .Bind(builder.Configuration.GetSection("AUP"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<HubApplicationSettings>()
    .Bind(builder.Configuration.GetSection("HubApplications"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<DynamoDbSettings>()
    .Bind(builder.Configuration.GetSection("DynamoDb"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<GoogleAdminSettings>()
    .Bind(builder.Configuration.GetSection("GoogleAdminSettings"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<GoogleAnalyticsSettings>()
    .Bind(builder.Configuration.GetSection("GoogleAnalytics"))
    .ValidateDataAnnotations();

builder.Services.AddTransient<IUserRepository, DynamoDbUserRepository>();

if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("DevelopmentModeUserRepository:Enabled"))
{
    builder.Services.AddOptions<DevelopmentModeUserRepositorySettings>()
        .Bind(builder.Configuration.GetSection("DevelopmentModeUserRepository"))
        .ValidateDataAnnotations();
    builder.Services.AddTransient<IUserRepository, NullUserRepository>();
}

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<GoogleAdminSettings>>();
    var googleKeyJson = config.Value.KeyJson;
    var adminToImpersonate = config.Value.AdminToImpersonate;

    return CredentialFactory.FromJson<ServiceAccountCredential>(googleKeyJson)
        .ToGoogleCredential()
        .CreateScoped(DirectoryService.Scope.AdminDirectoryUserReadonly, LicensingService.Scope.AppsLicensing)
        .CreateWithUser(adminToImpersonate);
});

// 2. Register the DirectoryService as a singleton, using the credential
builder.Services.AddSingleton(sp =>
{
    var credential = sp.GetRequiredService<GoogleCredential>();

    return new DirectoryService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "NIHR Hub",
    });
});

builder.Services.AddSingleton(sp =>
{
    var credential = sp.GetRequiredService<GoogleCredential>();

    return new LicensingService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "NIHR Hub",
    });
});

builder.Services.AddTransient<IGoogleAdminService, GoogleAdminService>();

builder.Services.AddTransient<IContentProvider, StaticContentProvider>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHealthChecks("/api/health");

app.Run();