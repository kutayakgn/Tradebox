using System;
using CMS.DocumentEngine;

namespace Tradebox.Models.PageTypes
{
    public partial class SSS : TreeNode
    {
        public const string CLASS_NAME = "Tradebox.SSS";

        public int PageSize
        {
            get => GetIntegerValue("PageSize", 10);
            set => SetValue("PageSize", value);
        }

        public bool IsFilterEnabled
        {
            get => GetBooleanValue("IsFilterEnabled", true);
            set => SetValue("IsFilterEnabled", value);
        }

        /// <summary>
        /// CMS panelden sabit kategori secimi.
        /// 0 veya bos ise tum kategoriler gelir, dolu ise yalnizca o kategori.
        /// </summary>
        public int ChosenCategory
        {
            get => GetIntegerValue("ChosenCategory", 0);
            set => SetValue("ChosenCategory", value);
        }
    }
}
