using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Nihr.Hub.Web.Helpers;

/// <summary>
/// Represents a single item in the breadcrumb trail.
/// </summary>
public class BreadcrumbItem
{
    public required string Text { get; init; }
    public required string Url { get; init; }
}

/// <summary>
/// HTML Helpers for generating GOV.UK Frontend components.
/// </summary>
public static class GovUkHtmlHelpers
{
    /// <summary>
    /// Generates a GOV.UK-styled breadcrumb navigation component.
    /// </summary>
    /// <param name="helper">The IHtmlHelper instance.</param>
    /// <param name="items">A list of BreadcrumbItem objects.</param>
    /// <returns>An IHtmlContent representing the breadcrumb navigation.</returns>
    public static IHtmlContent GovUkBreadcrumbs(this IHtmlHelper helper, IEnumerable<BreadcrumbItem> items)
    {
        var breadcrumbItems = items.ToList();
        if (breadcrumbItems.Count == 0)
        {
            return HtmlString.Empty;
        }

        // Create the main <nav> element
        var navTag = new TagBuilder("nav");
        navTag.AddCssClass("govuk-breadcrumbs");
        navTag.Attributes.Add("aria-label", "Breadcrumb");

        // Create the <ol> list
        var olTag = new TagBuilder("ol");
        olTag.AddCssClass("govuk-breadcrumbs__list");

        var lastItem = breadcrumbItems.Last();

        foreach (var item in breadcrumbItems)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("govuk-breadcrumbs__list-item");

            if (item == lastItem)
            {
                // Per GDS standard, the last item is the current page and not a link
                liTag.Attributes.Add("aria-current", "page");
                liTag.InnerHtml.Append(item.Text);
            }
            else
            {
                // All other items are links
                var aTag = new TagBuilder("a");
                aTag.AddCssClass("govuk-breadcrumbs__link");
                // Ensure there's a fallback href if URL is null/empty
                aTag.Attributes.Add("href", string.IsNullOrEmpty(item.Url) ? "#" : item.Url);
                aTag.InnerHtml.Append(item.Text);

                liTag.InnerHtml.AppendHtml(aTag);
            }

            olTag.InnerHtml.AppendHtml(liTag);
        }

        navTag.InnerHtml.AppendHtml(olTag);

        // Render the tag builder to an HtmlString
        using var writer = new StringWriter();
        navTag.WriteTo(writer, HtmlEncoder.Default);
        return new HtmlString(writer.ToString());
    }
}