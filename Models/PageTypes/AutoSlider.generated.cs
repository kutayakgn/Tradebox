using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class AutoSlider : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.AutoSlider";

        public string Title
        {
            get => GetStringValue("Title", string.Empty);
            set => SetValue("Title", value);
        }

        public int Interval
        {
            get => GetIntegerValue("Interval", 5000);
            set => SetValue("Interval", value);
        }

        public bool AutoPlay
        {
            get => GetBooleanValue("AutoPlay", true);
            set => SetValue("AutoPlay", value);
        }

        public string CssClass
        {
            get => GetStringValue("CssClass", string.Empty);
            set => SetValue("CssClass", value);
        }
    }
}
