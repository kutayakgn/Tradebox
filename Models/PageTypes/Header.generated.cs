using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class Header : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.Header";

        public string BrandImageUrl
        {
            get => GetStringValue("BrandImageUrl", string.Empty);
            set => SetValue("BrandImageUrl", value);
        }

        public string BrandHref
        {
            get => GetStringValue("BrandHref", "/");
            set => SetValue("BrandHref", value);
        }

        public string ApplyButtonText
        {
            get => GetStringValue("ApplyButtonText", string.Empty);
            set => SetValue("ApplyButtonText", value);
        }

        public string ApplyButtonIconUrl
        {
            get => GetStringValue("ApplyButtonIconUrl", string.Empty);
            set => SetValue("ApplyButtonIconUrl", value);
        }

        public string ModalButtonIconUrl
        {
            get => GetStringValue("ModalButtonIconUrl", string.Empty);
            set => SetValue("ModalButtonIconUrl", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
