using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Repositories;
using Nihr.Hub.Infrastructure.Settings;

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
    .AddCookie("Cookies") // Use cookies for session tracking
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

builder.Services.AddOptions<AupSettings>()
    .Bind(builder.Configuration.GetSection("AUP"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<HubApplicationSettings>()
    .Bind(builder.Configuration.GetSection("HubApplications"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<DynamoDbSettings>()
    .Bind(builder.Configuration.GetSection("DynamoDb"))
    .ValidateDataAnnotations();


builder.Services.AddTransient<IUserRepository, DynamoDbUserRepository>();

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