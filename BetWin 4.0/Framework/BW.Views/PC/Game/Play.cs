using BW.Views.IViews.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.PC.Game
{
    /// <summary>
    /// PC版的游戏中心(iframe嵌入）
    /// </summary>
    public sealed class Play : IPlay
    {
        public Play()
        {
        }

        public Play(string jsonString) : base(jsonString)
        {
        }
    }
}
