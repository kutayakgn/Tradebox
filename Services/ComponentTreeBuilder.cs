using CMS.DocumentEngine;
using Tradebox.Models.ViewModels;
using Tradebox.Repositories;

namespace Tradebox.Services;

public class ComponentTreeBuilder : IComponentTreeBuilder
{
    private readonly IComponentRepository _componentRepository;
    private readonly IComponentResolverService _componentResolver;
    private readonly ILogger<ComponentTreeBuilder> _logger;

    public ComponentTreeBuilder(
        IComponentRepository componentRepository,
        IComponentResolverService componentResolver,
        ILogger<ComponentTreeBuilder> logger)
    {
        _componentRepository = componentRepository;
        _componentResolver = componentResolver;
        _logger = logger;
    }

    /// <summary>
    /// Verilen parent path altindaki tum component'leri TEK SEFERDE DB'den ceker,
    /// sonra memory'de recursive tree'ye donusturur.
    ///
    /// ONCEKI YAKLASIM: Her seviye icin ayri DB sorgusu (N derinlik x M node)
    /// YENI YAKLASIM: 1 DB sorgusu + memory'de tree build
    /// </summary>
    public async Task<List<ComponentTreeNode>> BuildTreeAsync(
        string parentAliasPath,
        string culture = "tr-TR",
        int maxDepth = 10)
    {
        _logger.LogInformation(
            "Component agaci olusturuluyor: {Path}", parentAliasPath);

        // 1. DB'den tum alt node'lari tek seferde getir
        var allDescendants = await _componentRepository.GetAllDescendantsAsync(parentAliasPath, culture);

        if (!allDescendants.Any())
        {
            _logger.LogInformation("Alt component bulunamadi: {Path}", parentAliasPath);
            return new List<ComponentTreeNode>();
        }

        // 2. Memory'de tree olustur (DB sorgusu yok)
        var tree = BuildTreeInMemory(allDescendants, parentAliasPath, maxDepth);

        _logger.LogInformation(
            "Component agaci tamamlandi: {Path}, Root: {RootCount}, Toplam node: {TotalCount}",
            parentAliasPath, tree.Count, allDescendants.Count);

        return tree;
    }

    /// <summary>
    /// Flat node listesini parent-child iliskisine gore memory'de tree'ye donusturur.
    /// NodeParentID kullanarak her node'un parent'ini bulur.
    /// </summary>
    private List<ComponentTreeNode> BuildTreeInMemory(
        List<TreeNode> allNodes,
        string rootParentPath,
        int maxDepth)
    {
        // NodeID -> TreeNode lookup (hizli erisim)
        var nodeMap = allNodes.ToDictionary(n => n.NodeID, n => n);

        // NodeID -> ComponentTreeNode lookup
        var treeNodeMap = new Dictionary<int, ComponentTreeNode>();

        // Tum node'lari ComponentTreeNode'a donustur
        foreach (var node in allNodes)
        {
            var viewComponentName = _componentResolver.ResolveViewComponentName(node.ClassName);

            treeNodeMap[node.NodeID] = new ComponentTreeNode
            {
                Component = node,
                ViewComponentName = viewComponentName,
                Depth = 0, // asagida hesaplanacak
                Children = new List<ComponentTreeNode>()
            };
        }

        // Root node'lari (parent'i rootParentPath olan) ve child iliskilerini kur
        var rootNodes = new List<ComponentTreeNode>();

        foreach (var node in allNodes)
        {
            var treeNode = treeNodeMap[node.NodeID];

            // Bu node'un parent'i bizim listede var mi?
            var parentId = node.NodeParentID;

            if (nodeMap.ContainsKey(parentId) && treeNodeMap.ContainsKey(parentId))
            {
                // Parent var → child olarak ekle
                var parentTreeNode = treeNodeMap[parentId];
                treeNode.Depth = parentTreeNode.Depth + 1;

                // maxDepth kontrolu
                if (treeNode.Depth < maxDepth)
                {
                    parentTreeNode.Children.Add(treeNode);
                }
                else
                {
                    _logger.LogWarning(
                        "Maksimum derinlige ulasildi ({MaxDepth}): {Path}",
                        maxDepth, node.NodeAliasPath);
                }
            }
            else
            {
                // Parent listede yok → root node
                treeNode.Depth = 0;
                rootNodes.Add(treeNode);
            }
        }

        return rootNodes;
    }
}
