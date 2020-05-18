using Newtonsoft.Json;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BW.Views
{
    public abstract class IViewBase : IJsonSetting
    {
        public IViewBase()
        {
        }

        public IViewBase(string jsonString) : base(jsonString)
        {
        }

        /// <summary>
        /// 对前台显示的配置内容
        /// </summary>
        /// <returns></returns>
        public virtual JsonString ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
