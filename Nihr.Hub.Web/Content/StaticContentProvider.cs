using Microsoft.Extensions.Options;
using NIHR.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Settings;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Content;

/// <summary>
/// Hard-coded content provider seam. Replace this registration with a CMS-backed
/// implementation when content management is introduced.
/// </summary>
public class StaticContentProvider(IOptions<BannerSettings> bannerOptions) : IContentProvider
{
    private static readonly PoliciesContent PoliciesContent = new()
    {
        Policies =
        [
            new PolicyEntry
            {
                Title = "IT Policies",
                Url = "https://sites.google.com/nihr.ac.uk/nihr-induction-and-materials/it-support-and-policies/it-policies",
                Description = "NIHR IT policies covering acceptable use, data handling, and information security requirements for all staff."
            }
        ]
    };

    public Task<TContent> GetContentAsync<TContent>(string contentId, CancellationToken cancellationToken = default)
        where TContent : new()
    {
        return Task.FromResult(ResolveContent<TContent>(contentId));
    }

    public Task<TContent> GetContentAsync<TContent>(string contentId, string contentType, CancellationToken cancellationToken = default)
        where TContent : new()
    {
        return Task.FromResult(ResolveContent<TContent>(contentId));
    }

    private TContent ResolveContent<TContent>(string contentId)
        where TContent : new()
    {
        object? content = contentId switch
        {
            ContentIds.Policies => PoliciesContent,
            ContentIds.Banner => new BannerContent { Message = bannerOptions.Value.Message },
            _ => throw new InvalidOperationException($"No static content registered for content ID '{contentId}'.")
        };

        if (content is TContent typed)
            return typed;

        throw new InvalidCastException(
            $"Content for '{contentId}' is of type '{content.GetType().Name}', not '{typeof(TContent).Name}'.");
    }
}

public static class ContentIds
{
    public const string Policies = "policies";
    public const string Banner = "banner";
}
