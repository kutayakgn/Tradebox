using CMS.DocumentEngine;

namespace Tradebox.Models.ViewModels;


public class ComponentTreeNode
{

    public TreeNode Component { get; set; } = null!;


    public List<ComponentTreeNode> Children { get; set; } = new();

    public string ViewComponentName { get; set; } = string.Empty;


    public int Depth { get; set; }

    public bool HasChildren => Children.Count > 0;


    public string NodeAliasPath => Component.NodeAliasPath;
}
