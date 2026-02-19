using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class RouteItem : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.RouteItem";

        public string Title
        {
            get => GetStringValue("Title", string.Empty);
            set => SetValue("Title", value);
        }

        public string Url
        {
            get => GetStringValue("Url", string.Empty);
            set => SetValue("Url", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
