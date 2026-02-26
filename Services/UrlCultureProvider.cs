using Microsoft.AspNetCore.Localization;

namespace Tradebox.Services;

/// <summary>
/// URL path'inde "en-us" segmenti varsa kültürü "en-US" olarak belirler,
/// yoksa null döner (bir sonraki provider'a — genellikle Cookie — geçilir).
///
/// Ayrıca URL'den kültür tespiti yapıldığında bunu cookie'ye de yazar;
/// böylece dil seçimi bir sonraki ziyarette de korunur.
/// </summary>
public class UrlCultureProvider : IRequestCultureProvider
{
    private const string EnUsSegment = "en-us";
    private const string EnUsCulture = "en-US";
    private const string TrTrCulture = "tr-TR";

    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value ?? string.Empty;

        // Segmentlere ayırarak kesin eşleşme kontrol et (örn. /en-us veya /en-us/sayfa)
        var hasEnUs = path
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Any(s => s.Equals(EnUsSegment, StringComparison.OrdinalIgnoreCase));

        if (!hasEnUs)
            return Task.FromResult<ProviderCultureResult?>(null); // Cookie provider'a bırak

        // URL'den tespit edilen dili cookie'ye yaz (kalıcı tercih için)
        httpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(EnUsCulture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(EnUsCulture));
    }
}
