using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class HeroBanner : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.HeroBanner";

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

        public string BackgroundImageUrl
        {
            get => GetStringValue("BackgroundImageUrl", string.Empty);
            set => SetValue("BackgroundImageUrl", value);
        }

        public string ButtonText
        {
            get => GetStringValue("ButtonText", string.Empty);
            set => SetValue("ButtonText", value);
        }

        public string ButtonUrl
        {
            get => GetStringValue("ButtonUrl", string.Empty);
            set => SetValue("ButtonUrl", value);
        }
    }
}