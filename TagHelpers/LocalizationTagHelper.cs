using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Tradebox.Services;

namespace Tradebox.TagHelpers;

/// <summary>
/// Kentico resource string'lerini Razor view'larında doğrudan tag olarak kullanmayı sağlar.
///
/// Kullanım örnekleri:
///   <loc key="general.yes" />                          → sadece metin
///   <loc key="nav.home" tag="span" />                  → <span>Anasayfa</span>
///   <loc key="nav.home" tag="span" class="bold" />     → <span class="bold">Anasayfa</span>
///   <loc key="header.title" culture="en-US" />         → belirli kültür
///   <loc key="welcome.user" format="Ahmet" />          → string.Format ile {0}
/// </summary>
[HtmlTargetElement("loc")]
public class LocalizationTagHelper : TagHelper
{
    private readonly IKenticoLocalizationService _loc;

    public LocalizationTagHelper(IKenticoLocalizationService loc)
    {
        _loc = loc;
    }

    /// <summary>Kentico resource string key'i.</summary>
    [HtmlAttributeName("key")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Opsiyonel: çıktıyı sarmak istenen HTML tag.
    /// Belirtilmezse sadece metin olarak render edilir.
    /// </summary>
    [HtmlAttributeName("tag")]
    public string? Tag { get; set; }

    /// <summary>Opsiyonel: kültür kodu (örn. "tr-TR", "en-US"). Belirtilmezse aktif kültür kullanılır.</summary>
    [HtmlAttributeName("culture")]
    public string? Culture { get; set; }

    /// <summary>
    /// Opsiyonel: virgülle ayrılmış format argümanları.
    /// Örnek: format="Ahmet,5"  → string.Format(value, "Ahmet", "5")
    /// </summary>
    [HtmlAttributeName("format")]
    public string? Format { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <loc> elementini gizle; tag parametresine göre değiştir veya sadece metin yaz
        if (string.IsNullOrWhiteSpace(Tag))
        {
            output.TagName = null; // tag üret, sadece içerik yaz
        }
        else
        {
            output.TagName = Tag;
        }

        output.TagMode = TagMode.StartTagAndEndTag;

        // Resource string'i al
        var value = string.IsNullOrEmpty(Culture)
            ? _loc.Get(Key)
            : _loc.Get(Key, Culture);

        // Format argümanları varsa uygula
        if (!string.IsNullOrEmpty(Format))
        {
            var args = Format.Split(',').Select(a => (object)a.Trim()).ToArray();
            value = string.Format(value, args);
        }

        output.Content.SetContent(value);
    }
}
