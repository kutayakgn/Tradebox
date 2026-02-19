using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    /// <summary>
    /// Header "Basvuru" modalindaki bolumler (chapter).
    /// Her chapter bir paragraf ve altinda butonlar icerir.
    ///
    /// Ornek CMS tree:
    /// ApplyModal (Folder)
    ///   ├── Bireysel Hesap (HeaderApplyChapter) → Paragraph: "Bireysel hesap acmak icin..."
    ///   │   ├── Online Basvuru (HeaderApplyButton) → Href: /basvuru/bireysel
    ///   │   └── Sube Randevu (HeaderApplyButton) → Href: /randevu
    ///   └── Kurumsal Hesap (HeaderApplyChapter) → Paragraph: "Kurumsal hesap icin..."
    ///       └── Online Basvuru (HeaderApplyButton) → Href: /basvuru/kurumsal
    /// </summary>
    public partial class HeaderApplyChapter : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.HeaderApplyChapter";

        public string Paragraph
        {
            get => GetStringValue("Paragraph", string.Empty);
            set => SetValue("Paragraph", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
