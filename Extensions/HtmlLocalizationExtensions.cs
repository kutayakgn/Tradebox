using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tradebox.Services;

namespace Tradebox.Extensions;

/// <summary>
/// IHtmlHelper üzerine Kentico localization extension metodları.
///
/// Kullanım örnekleri (.cshtml içinde):
///   @Html.Loc("general.yes")
///   @Html.Loc("welcome.user", "Ahmet")
///   @Html.Loc("header.title", culture: "en-US")
///   @Html.LocRaw("content.html")         → HTML encode edilmez (ham HTML içerik için)
/// </summary>
public static class HtmlLocalizationExtensions
{
    /// <summary>
    /// Kentico'dan resource string'i alır, HTML-encode ederek yazar.
    /// </summary>
    public static IHtmlContent Loc(this IHtmlHelper html, string key)
    {
        var loc = GetService(html);
        return new HtmlString(Encode(loc.Get(key)));
    }

    /// <summary>
    /// Resource string'i alır ve String.Format ile formatlar.
    /// </summary>
    public static IHtmlContent Loc(this IHtmlHelper html, string key, params object[] args)
    {
        var loc = GetService(html);
        return new HtmlString(Encode(loc.GetFormat(key, args)));
    }

    /// <summary>
    /// Belirtilen kültür için resource string'i alır.
    /// </summary>
    public static IHtmlContent Loc(this IHtmlHelper html, string key, string culture)
    {
        var loc = GetService(html);
        return new HtmlString(Encode(loc.Get(key, culture)));
    }

    /// <summary>
    /// Resource string'i HTML-encode ETMEDEN yazar. Ham HTML içerikler için kullanın.
    /// </summary>
    public static IHtmlContent LocRaw(this IHtmlHelper html, string key)
    {
        var loc = GetService(html);
        return new HtmlString(loc.Get(key));
    }

    /// <summary>
    /// "{$key$}" kalıplarını içeren metni lokalize eder.
    /// </summary>
    public static IHtmlContent LocText(this IHtmlHelper html, string text)
    {
        var loc = GetService(html);
        return new HtmlString(Encode(loc.Localize(text)));
    }

    // -------------------------------------------------------

    private static IKenticoLocalizationService GetService(IHtmlHelper html)
    {
        return html.ViewContext.HttpContext
            .RequestServices
            .GetRequiredService<IKenticoLocalizationService>();
    }

    private static string Encode(string value)
    {
        return System.Net.WebUtility.HtmlEncode(value);
    }
}
