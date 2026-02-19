using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class Footer : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.Footer";

        public string CopyrightText
        {
            get => GetStringValue("CopyrightText", string.Empty);
            set => SetValue("CopyrightText", value);
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
