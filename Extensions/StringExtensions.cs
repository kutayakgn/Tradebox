namespace Tradebox.Extensions;

public static class StringExtensions
{
    public static string ToNodeAliasPath(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "/";

        url = url.Trim('/');
        return "/" + url;
    }
}