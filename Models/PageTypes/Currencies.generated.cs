using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class Currencies : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.Currencies";

        public string Title
        {
            get => GetStringValue("Title", string.Empty);
            set => SetValue("Title", value);
        }

        public string BaseCurrency
        {
            get => GetStringValue("BaseCurrency", string.Empty);
            set => SetValue("BaseCurrency", value);
        }

        public string DisplayCurrencies
        {
            get => GetStringValue("DisplayCurrencies", string.Empty);
            set => SetValue("DisplayCurrencies", value);
        }

        public int RefreshInterval
        {
            get => GetIntegerValue("RefreshInterval", 60000);
            set => SetValue("RefreshInterval", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
