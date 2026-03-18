using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Nihr.Hub.Web.TagHelpers;

public class ServiceNavigationItemContext
{
    public bool IsActive { get; set; }
}

[HtmlTargetElement("li")]
public class ServiceNavigationItemTagHelper : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context?.AllAttributes["class"]?.Value?.ToString()?.Split(' ')?.Contains("govuk-service-navigation__item", StringComparer.OrdinalIgnoreCase) ?? false)
        {
            var itemContext = new ServiceNavigationItemContext();

            context.Items.Add(typeof(ServiceNavigationItemContext), itemContext);

            await output.GetChildContentAsync();

            if (itemContext.IsActive)
            {
                output.AddClass("govuk-service-navigation__item--active", HtmlEncoder.Default);
            }
        }
    }
}


[HtmlTargetElement("a", ParentTag = "li", Attributes = "asp-controller, asp-action")]
public class ChildTagHelper : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public string? AspController { get; set; }
    public string? AspAction { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var currentController = ViewContext?.RouteData.Values["controller"]?.ToString();
        var currentAction = ViewContext?.RouteData.Values["action"]?.ToString();

        if (string.Equals(currentController, AspController, StringComparison.OrdinalIgnoreCase)
            && string.Equals(currentAction, AspAction, StringComparison.OrdinalIgnoreCase))
        {
            if (context.Items[typeof(ServiceNavigationItemContext)] is ServiceNavigationItemContext itemContext)
            {
                itemContext.IsActive = true;
            }

            output.Attributes.SetAttribute("aria-current", "true");

            output.PreContent.AppendHtml("<strong class=\"govuk-service-navigation__active-fallback\">");
            output.PostContent.AppendHtml("</strong>");
        }
    }
}