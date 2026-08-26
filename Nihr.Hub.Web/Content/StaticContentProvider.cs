using Microsoft.Extensions.Options;
using NIHR.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Settings;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Content;

/// <summary>
/// Config-backed content provider seam. Replace this registration with a CMS-backed
/// implementation when content management is introduced (per ADR-0001).
/// </summary>
public class StaticContentProvider(IOptions<PoliciesSettings> policiesOptions) : IContentProvider
{
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
            ContentIds.Policies => BuildPoliciesContent(),
            _ => throw new InvalidOperationException($"No content registered for content ID '{contentId}'.")
        };

        if (content is TContent typed)
            return typed;

        throw new InvalidCastException(
            $"Content for '{contentId}' is of type '{content.GetType().Name}', not '{typeof(TContent).Name}'.");
    }

    private PoliciesContent BuildPoliciesContent() => new()
    {
        Policies = policiesOptions.Value.Items
            .Select(i => new PolicyEntry
            {
                Title = i.PolicyName,
                Url = i.PolicyUrl,
                Description = i.PolicyDescription
            })
            .ToList()
    };
}

public static class ContentIds
{
    public const string Policies = "policies";
    public const string Banner = "banner";
}
