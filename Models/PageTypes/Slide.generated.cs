using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class Slide : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.Slide";

        public string Title
        {
            get => GetStringValue("Title", string.Empty);
            set => SetValue("Title", value);
        }

        public string Description
        {
            get => GetStringValue("Description", string.Empty);
            set => SetValue("Description", value);
        }

        public string ImageUrl
        {
            get => GetStringValue("ImageUrl", string.Empty);
            set => SetValue("ImageUrl", value);
        }

        public string LinkUrl
        {
            get => GetStringValue("LinkUrl", string.Empty);
            set => SetValue("LinkUrl", value);
        }

        public string LinkText
        {
            get => GetStringValue("LinkText", string.Empty);
            set => SetValue("LinkText", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
