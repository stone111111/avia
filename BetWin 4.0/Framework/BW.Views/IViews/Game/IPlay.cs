using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.IViews.Game
{
    /// <summary>
    /// 游戏中心
    /// </summary>
    public abstract class IPlay : IViewBase
    {
        public IPlay()
        {
        }

        public IPlay(string jsonString) : base(jsonString)
        {
        }
    }
}
