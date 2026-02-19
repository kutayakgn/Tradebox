using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    /// <summary>
    /// Header "Basvuru" modalindaki butonlar.
    /// HeaderApplyChapter altinda yer alir.
    /// </summary>
    public partial class HeaderApplyButton : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.HeaderApplyButton";

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

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
