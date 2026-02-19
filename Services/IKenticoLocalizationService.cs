namespace Tradebox.Services;

/// <summary>
/// Kentico ResHelper üzerinden resource string erişimi sağlayan servis.
/// </summary>
public interface IKenticoLocalizationService
{
    /// <summary>
    /// Verilen key için aktif kültürdeki resource string'i döner.
    /// Bulunamazsa key'i döner.
    /// </summary>
    string Get(string key);

    /// <summary>
    /// Verilen key için belirtilen kültürdeki resource string'i döner.
    /// </summary>
    string Get(string key, string culture);

    /// <summary>
    /// Verilen key için resource string'i alır, ardından String.Format ile formatlar.
    /// Örnek: Get("mykey", "Ahmet", 5) → "Merhaba Ahmet, 5 ürün bulundu"
    /// </summary>
    string GetFormat(string key, params object[] args);

    /// <summary>
    /// Verilen key için belirtilen kültürde resource string'i alır ve formatlar.
    /// </summary>
    string GetFormat(string key, string culture, params object[] args);

    /// <summary>
    /// "{$key$}" kalıbındaki ifadeleri içeren metni lokalize eder.
    /// Örnek: "Merhaba {$greeting.user$}" → "Merhaba Kullanıcı"
    /// </summary>
    string Localize(string text);

    /// <summary>
    /// "{$key$}" kalıbındaki ifadeleri belirtilen kültürde lokalize eder.
    /// </summary>
    string Localize(string text, string culture);
}
