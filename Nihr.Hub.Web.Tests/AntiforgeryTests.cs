using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nihr.Hub.Web.Tests.Infrastructure;
using Xunit;

namespace Nihr.Hub.Web.Tests;

public class AntiforgeryTests : IClassFixture<NihrWebApplicationFactory>
{
    private readonly NihrWebApplicationFactory _factory;

    public AntiforgeryTests(NihrWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CookieBannerPost_WithoutAntiforgeryToken_IsRejected()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });

        var request = new HttpRequestMessage(HttpMethod.Post, "/cookies/accept")
        {
            Content = new FormUrlEncodedContent([])
        };
        request.Headers.Referrer = new Uri("https://localhost/cookies");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CookieBannerPost_WithAntiforgeryToken_Succeeds()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });

        var getResponse = await client.GetAsync("/cookies");
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = GetAntiforgeryToken(html);

        var request = new HttpRequestMessage(HttpMethod.Post, "/cookies/accept")
        {
            Content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("__RequestVerificationToken", token)
            ])
        };
        request.Headers.Referrer = new Uri("https://localhost/cookies");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    [Fact]
    public void FavouritesScript_SendsConfiguredAntiforgeryHeader()
    {
        var environment = _factory.Services.GetRequiredService<IHostEnvironment>();
        var scriptPath = Path.Combine(environment.ContentRootPath, "wwwroot", "js", "favourite-apps.js");

        var script = File.ReadAllText(scriptPath);

        Assert.Contains("request-verification-token", script);
        Assert.Contains("RequestVerificationToken", script);
    }

    private static string GetAntiforgeryToken(string html)
    {
        var match = Regex.Match(
            html,
            "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"",
            RegexOptions.IgnoreCase);

        Assert.True(match.Success, "Expected antiforgery token input was not rendered in HTML response.");

        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }
}


