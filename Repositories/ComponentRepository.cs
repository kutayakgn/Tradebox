using System.Reflection;
using CMS.DocumentEngine;

namespace Tradebox.Repositories;

public class ComponentRepository : IComponentRepository
{
    private readonly ILogger<ComponentRepository> _logger;

    // Reflection ile otomatik kesfedilen typed query map (uygulama basinda 1 kez olusur)
    private static readonly Dictionary<string, Func<string, string, List<TreeNode>>> _typedQueryMap
        = new(StringComparer.OrdinalIgnoreCase);

    private static bool _initialized;
    private static readonly object _initLock = new();

    static ComponentRepository()
    {
        InitializeTypedQueryMap();
    }

    public ComponentRepository(ILogger<ComponentRepository> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Parent path altindaki TUM node'lari tek sorguda getirir.
    /// Once hafif bir sorgu ile NodeID + ClassName + NodeAliasPath + NodeParentID alinir,
    /// sonra her ClassName icin typed query ile sadece o tipe ait kolonlar cekilir.
    ///
    /// ONCEKI YAKLASIM: Her seviye icin ayri sorgu (N derinlik x M node = cok fazla DB sorgusu)
    /// YENI YAKLASIM: Tum derinlikler tek seferde (max 2-3 sorgu: 1 index + ClassName basina 1)
    /// </summary>
    public Task<List<TreeNode>> GetAllDescendantsAsync(
        string parentAliasPath, string culture = "tr-TR")
    {
        var allComponents = new List<TreeNode>();

        try
        {
            var descendantPath = parentAliasPath.TrimEnd('/') + "/%";

            // 1. ADIM: Hafif index sorgusu - sadece yapısal bilgiler
            // Tum derinliklerdeki node'lari tek seferde getirir (NestingLevel yok)
            var indexDocs = DocumentHelper.GetDocuments()
                .Path(descendantPath)
                .Culture(culture)
                .CombineWithDefaultCulture()
                .Published()
                .LatestVersion(false)
                .OnCurrentSite()
                .OrderBy("NodeLevel", "NodeOrder")
                .Columns("NodeID", "ClassName", "NodeOrder", "NodeAliasPath",
                         "NodeParentID", "NodeLevel", "DocumentName")
                .ToList();

            if (!indexDocs.Any())
                return Task.FromResult(allComponents);

            // 2. ADIM: ClassName'lere gore gruplandir ve typed query calistir
            var classNames = indexDocs
                .Select(d => d.ClassName)
                .Distinct()
                .ToList();

            var typedResults = new Dictionary<int, TreeNode>();

            foreach (var className in classNames)
            {
                if (_typedQueryMap.TryGetValue(className, out var queryFunc))
                {
                    var typedDocs = queryFunc(descendantPath, culture);
                    foreach (var doc in typedDocs)
                    {
                        typedResults[doc.NodeID] = doc;
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "Typed query bulunamadi: {ClassName}. " +
                        "PageType class'inda CLASS_NAME const tanimli mi kontrol edin.",
                        className);
                }
            }

            // 3. ADIM: Index sirasina gore typed sonuclari sirala
            foreach (var indexDoc in indexDocs)
            {
                if (typedResults.TryGetValue(indexDoc.NodeID, out var typedNode))
                {
                    allComponents.Add(typedNode);
                }
            }

            _logger.LogInformation(
                "Tum descendants yuklendi: {Path}, Toplam: {Count}, Tipler: {Types}",
                parentAliasPath,
                allComponents.Count,
                string.Join(", ", classNames));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Descendants getirilemedi: {ParentPath}", parentAliasPath);
        }

        return Task.FromResult(allComponents);
    }

    /// <summary>
    /// Uygulama basinda 1 kez calisir. Assembly'deki tum TreeNode subclass'larini tarar,
    /// CLASS_NAME const'i olan tipleri bulur ve typed query fonksiyonlarini olusturur.
    /// </summary>
    private static void InitializeTypedQueryMap()
    {
        if (_initialized) return;

        lock (_initLock)
        {
            if (_initialized) return;

            var treeNodeType = typeof(TreeNode);
            var assembly = Assembly.GetExecutingAssembly();

            var pageTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && treeNodeType.IsAssignableFrom(t)
                    && t != treeNodeType);

            foreach (var type in pageTypes)
            {
                var classNameField = type.GetField("CLASS_NAME",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (classNameField == null || classNameField.FieldType != typeof(string))
                    continue;

                var className = classNameField.GetValue(null) as string;
                if (string.IsNullOrEmpty(className))
                    continue;

                var queryMethod = typeof(ComponentRepository)
                    .GetMethod(nameof(QueryTyped), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(type);

                _typedQueryMap[className] = (path, culture) =>
                    (List<TreeNode>)queryMethod.Invoke(null, new object[] { path, culture })!;
            }

            _initialized = true;
        }
    }

    /// <summary>
    /// Typed query - sadece o tipe ait kolonlari getirir.
    /// NestingLevel KULLANILMAZ cunku tum derinlikleri tek seferde cekmek istiyoruz.
    /// </summary>
    private static List<TreeNode> QueryTyped<T>(string path, string culture)
        where T : TreeNode, new()
    {
        return DocumentHelper.GetDocuments<T>()
            .Path(path)
            .Culture(culture)
            .CombineWithDefaultCulture()
            .Published()
            .LatestVersion(false)
            .OnCurrentSite()
            .OrderBy("NodeLevel", "NodeOrder")
            .ToList()
            .Cast<TreeNode>()
            .ToList();
    }
}
