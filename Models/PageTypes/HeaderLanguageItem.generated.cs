using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    /// <summary>
    /// Header dil secenekleri (TR, EN vb.)
    ///
    /// Ornek CMS tree:
    /// Languages (Folder)
    ///   ├── Turkce (HeaderLanguageItem) → Href: /, ImageUrl: /assets/flags/tr.svg, Code: tr
    ///   └── English (HeaderLanguageItem) → Href: /en-us, ImageUrl: /assets/flags/en.svg, Code: en
    /// </summary>
    public partial class HeaderLanguageItem : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.HeaderLanguageItem";

        public string Text
        {
            get => GetStringValue("Text", string.Empty);
            set => SetValue("Text", value);
        }

        public string Href
        {
            get => GetStringValue("Href", string.Empty);
            set => SetValue("Href", value);
        }

        public string ImageUrl
        {
            get => GetStringValue("ImageUrl", string.Empty);
            set => SetValue("ImageUrl", value);
        }

        public string Code
        {
            get => GetStringValue("Code", string.Empty);
            set => SetValue("Code", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
