namespace Tradebox.Services;

public interface IComponentResolverService
{
    string ResolveViewComponentName(string className);
}