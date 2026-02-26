using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Tradebox.Extensions;
using Tradebox.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────
// 1) Kentico Xperience 13 Services
// ──────────────────────────────────────────────
builder.Services.AddKentico(features =>
{
    features.UsePageBuilder();
    features.UsePageRouting();
});

// ──────────────────────────────────────────────
// 2) MVC + Razor
// ──────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ──────────────────────────────────────────────
// 3) Custom Services (Repository, Helper vb.)
// ──────────────────────────────────────────────
builder.Services.AddTradeboxServices();

var app = builder.Build();

// ──────────────────────────────────────────────
// Kentico Initialization (minimal API hosting modeli için zorunlu)
// ──────────────────────────────────────────────
app.InitKentico();

// ──────────────────────────────────────────────
// Middleware Pipeline
// ──────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ──────────────────────────────────────────────
// Localization: URL → Cookie → varsayılan (tr-TR)
// ──────────────────────────────────────────────
var supportedCultures = new[] { "tr-TR", "en-US" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("tr-TR")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .AddInitialRequestCultureProvider(new Tradebox.Services.UrlCultureProvider()));

// 301 Redirect (Custom Table'dan, gunluk cache)
app.UseMiddleware<RedirectMiddleware>();

app.UseRouting();

// Kentico middleware
app.UseKentico();

// Custom global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// Request logging (only in Development)
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<RequestLoggingMiddleware>();
}

app.UseAuthorization();

// ──────────────────────────────────────────────
// Route Mapping
// ──────────────────────────────────────────────
app.UseEndpoints(endpoints =>
{
    endpoints.Kentico().MapRoutes();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
