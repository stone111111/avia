using GM.Common.Systems;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SP.StudioCore.API;
using SP.StudioCore.Enums;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.System.Agent.Systems;
using Web.System.Utils;

namespace Web.System.Handler.Systems
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Route("system/[controller]/[action]")]
    public class ConfigController : SysControllerBase
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        /// <returns></returns>
        [HttpPost, Guest]
        public Result Init()
        {
            Dictionary<string, Dictionary<string, string>> @enum = new Dictionary<string, Dictionary<string, string>>();
            foreach (Assembly assembly in new[]
            {
                typeof(EnumExtensions).Assembly,
                typeof(GM.Common.Setting).Assembly,
                //typeof(GM.Views.ViewUtils).Assembly,
                //typeof(SP.Provider.CDN.ICDNProvider).Assembly,
                this.GetType().Assembly
            })
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> t in assembly.GetEnums())
                {
                    if (!@enum.ContainsKey(t.Key)) @enum.Add(t.Key, t.Value);
                }
            }
            //@enum[typeof(Language).FullName] = SettingAgent.Instance().GetSetting(SystemSetting.SettingType.Language)?.
            //    Split(',').Select(t => t.ToEnum<Language>()).Distinct().ToDictionary(t => t.ToString(), t => t.GetDescription());
            return this.GetResultContent(new
            {
                Enum = @enum,
                ImgServer = APIAgent.imgServer
            });
        }

        /// <summary>
        /// layupload 组件的图片上传接口
        /// </summary>
        /// <returns></returns>
        public Task Layupload()
        {
            if (HttpContext.Request.Form.Files.Count == 0)
            {
                return this.GetResult(false, "没有选择要上传的图片");
            }
            Result result = HttpContext.Request.Form.Files[0].Upload();
            if (result.Success != 1)
            {
                return this.GetResult(false, result.Message);
            }
            string fileName = ((JObject)result.Info)["fileName"].Value<string>();

            return new Result(ContentType.JSON, new
            {
                code = 0,
                msg = this.StopwatchMessage(),
                data = new
                {
                    value = fileName,
                    src = fileName.GetImage()
                }
            }).WriteAsync(HttpContext);
        }

        #region ========  公共配置参数读取  ========

        /// <summary>
        /// 获取系统当前开放的CDN供应商
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public Task CDNProvider()
        //{
        //    return this.GetResult(ProviderAgent.Instance().GetCDNProviders().Select(t => t.Type));
        //}

        #endregion

        #region ========  系统运维中的参数设定  ========

        /// <summary>
        /// 获取系统全局的参数配置
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统运维.参数配置.Value)]
        public Task GetSettings()
        {
            return null;
        }


        ///// <summary>
        ///// 保存系统的参数配置
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost, Permission(GM.Permission.系统运维.参数配置.Value)]
        //public Task SaveSetting([FromForm]SystemSetting.SettingType type, [FromForm]string value)
        //{
        //    return this.GetResult(SettingAgent.Instance().SaveSetting(type, value));
        //}

        #endregion
    }
}
