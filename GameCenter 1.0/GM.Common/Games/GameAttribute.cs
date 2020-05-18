using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.Games
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class GameAttribute : Attribute
    {
        public GameAttribute(CategoryType categories)
        {
            this.Categories = categories;
        }

        /// <summary>
        /// 包含的种类
        /// </summary>
        public CategoryType Categories { get; set; }
    }
}
