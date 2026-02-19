using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class Page : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.Page";

        public string PageName
        {
            get => GetStringValue("PageName", string.Empty);
            set => SetValue("PageName", value);
        }
    }
}
