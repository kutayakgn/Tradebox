using CMS.Helpers;
using CMS.Localization;

namespace Tradebox.Services;

/// <summary>
/// Kentico'nun ResHelper/LocalizationHelper altyapısını kullanan,
/// DI uyumlu localization servisi.
/// </summary>
public class KenticoLocalizationService : IKenticoLocalizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<KenticoLocalizationService> _logger;

    private const string DefaultCulture = "tr-TR";

    public KenticoLocalizationService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<KenticoLocalizationService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string Get(string key)
    {
        return Get(key, GetCurrentCulture());
    }

    public string Get(string key, string culture)
    {
        if (string.IsNullOrWhiteSpace(key))
            return string.Empty;

        try
        {
            var value = ResHelper.GetString(key, culture, useDefaultCulture: true);

            // ResHelper bulamazsa key'in kendisini döner; boş dönüşü key ile ele al
            return string.IsNullOrEmpty(value) ? key : value;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Localization key alınamadı: {Key} / {Culture}", key, culture);
            return key;
        }
    }

    public string GetFormat(string key, params object[] args)
    {
        return GetFormat(key, GetCurrentCulture(), args);
    }

    public string GetFormat(string key, string culture, params object[] args)
    {
        if (string.IsNullOrWhiteSpace(key))
            return string.Empty;

        try
        {
            var template = Get(key, culture);
            return args.Length > 0 ? string.Format(template, args) : template;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Localization format hatası: {Key}", key);
            return key;
        }
    }

    public string Localize(string text)
    {
        return Localize(text, GetCurrentCulture());
    }

    public string Localize(string text, string culture)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text ?? string.Empty;

        try
        {
            return LocalizationHelper.LocalizeString(text, culture);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Localization LocalizeString hatası: {Text}", text);
            return text;
        }
    }

    // -------------------------------------------------------
    // Yardımcı: request thread'indeki kültürü al, yoksa default
    // -------------------------------------------------------
    private string GetCurrentCulture()
    {
        try
        {
            var culture = _httpContextAccessor.HttpContext?
                .Request.HttpContext.Features
                .Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()
                ?.RequestCulture.Culture.Name;

            return string.IsNullOrEmpty(culture) ? DefaultCulture : culture;
        }
        catch
        {
            return DefaultCulture;
        }
    }
}
