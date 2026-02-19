using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class TextBlock : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.TextBlock";

        public string Heading
        {
            get => GetStringValue("Heading", string.Empty);
            set => SetValue("Heading", value);
        }

        public string Content
        {
            get => GetStringValue("Content", string.Empty);
            set => SetValue("Content", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}