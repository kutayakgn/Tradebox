using Tradebox.Repositories;
using Tradebox.Services;

namespace Tradebox.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTradeboxServices(this IServiceCollection services)
    {
        // Memory Cache (Commons, sayfa cache icin)
        services.AddMemoryCache();

        // Repositories
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IComponentRepository, ComponentRepository>();

        // Services
        services.AddScoped<IComponentTreeBuilder, ComponentTreeBuilder>();
        services.AddScoped<IPageBuilderService, PageBuilderService>();
        services.AddScoped<ICommonsService, CommonsService>();
        services.AddScoped<IComponentResolverService, ComponentResolverService>();
        services.AddSingleton<IRedirectService, RedirectService>();
        services.AddScoped<IBreadcrumbService, BreadcrumbService>();

        return services;
    }
}


