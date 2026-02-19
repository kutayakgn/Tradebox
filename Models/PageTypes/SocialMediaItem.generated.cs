using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class SocialMediaItem : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.SocialMediaItem";

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

        public string IconCssClass
        {
            get => GetStringValue("IconCssClass", string.Empty);
            set => SetValue("IconCssClass", value);
        }

        public string IconUrl
        {
            get => GetStringValue("IconUrl", string.Empty);
            set => SetValue("IconUrl", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
