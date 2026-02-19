using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    /// <summary>
    /// Header navigasyon menusundeki route ogeleri.
    /// CMS tree'de recursive olarak 3 seviye derinlige kadar child icerebilir.
    ///
    /// Ornek CMS tree:
    /// Routes (Folder)
    ///   ├── Bireysel (HeaderRouteItem) → Href: /bireysel
    ///   │   ├── Hesaplar (HeaderRouteItem) → Href: /bireysel/hesaplar
    ///   │   └── Kartlar (HeaderRouteItem) → Href: /bireysel/kartlar
    ///   ├── Kurumsal (HeaderRouteItem) → Href: /kurumsal
    ///   └── Hakkimizda (HeaderRouteItem) → Href: /hakkimizda
    /// </summary>
    public partial class HeaderRouteItem : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.HeaderRouteItem";

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
