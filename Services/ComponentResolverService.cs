namespace Tradebox.Services;

public class ComponentResolverService : IComponentResolverService
{
    public string ResolveViewComponentName(string className)
    {
        if (string.IsNullOrEmpty(className))
            return string.Empty;

        var dotIndex = className.LastIndexOf('.');
        return dotIndex >= 0 ? className.Substring(dotIndex + 1) : className;
    }
}