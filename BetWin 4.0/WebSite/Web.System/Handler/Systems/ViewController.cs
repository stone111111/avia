using BW.Common.Systems;
using BW.Common.Views;
using BW.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.System.Agent.Systems;
using Web.System.Utils;

namespace Web.System.Handler.Systems
{
    /// <summary>
    /// 视图管理
    /// </summary>
    [Route("system/[controller]/[action]")]
    public sealed class ViewController : SysControllerBase
    {
        #region ========  系统方法  ========

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task Initialize()
        {
            int count = ViewAgent.Instance().Initialize();
            return this.GetResult(true, $"找到{count}个视图配置");
        }

        #endregion

        #region ========  视图/模型 管理  ========

        /// <summary>
        /// 系统视图列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task SettingList([FromForm]PlatformSource? platform)
        {
            if (platform == null) platform = PlatformSource.PC;
            List<ViewSetting> list = BDC.ViewSetting.Where(t => t.Platform == platform.Value && t.Status == ViewSetting.ViewStatus.Normal).OrderByDescending(t => t.Sort).ToList();
            List<ViewModel> models = BDC.ViewModel.Where(t => BDC.ViewSetting.Any(p => p.ID == t.ViewID)).ToList();
            return this.GetResult(this.ShowResult(list, t => new
            {
                t.ID,
                t.Name,
                t.Code,
                Models = models.Where(p => p.ViewID == t.ID).Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.Preview,
                    p.Description
                })
            }));
        }

        /// <summary>
        /// 获取模型信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        [HttpPost]
        public Task GetModelInfo([FromForm]int? id, [FromForm]int? viewId)
        {
            ViewModel model = ViewAgent.Instance().GetModelInfo(id == null ? 0 : id.Value) ?? new ViewModel()
            {
                ViewID = viewId ?? 0
            };
            List<ViewContent> contents = BDC.ViewContent.Where(t => t.ModelID == model.ID).ToList();
            return this.GetResult(new
            {
                model.ID,
                model.Name,
                model.Path,
                model.Preview,
                model.ViewID,
                model.Description,
                Resources = model.ResourceFiles,
                Content = contents
            });
        }

        /// <summary>
        /// 获取模型页面列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task ModelContents([FromForm]int id)
        {
            var list = BDC.ViewContent.Where(t => t.ModelID == id);
            string cdn = SettingAgent.Instance().GetSetting(SystemSetting.SettingType.CDNUrl);
            return this.GetResult(this.ShowResult(list, t => new
            {
                t.ModelID,
                t.Language,
                Path = string.Concat(cdn, "/", t.Path),
                t.Translate
            }));
        }

        /// <summary>
        /// 保存视图模型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task SaveModelInfo()
        {
            ViewModel model = this.context.Request.Form.Fill<ViewModel>();
            return this.GetResult(ViewAgent.Instance().SaveModelInfo(model), "保存成功", new
            {
                model.ID
            });
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public Task SaveModelFile([FromForm]int id, [FromForm]IFormFile file, [FromForm]long modified)
        {
            string name = file.FileName;
            string ext = name.Substring(name.LastIndexOf('.') + 1);
            bool success;
            switch (ext)
            {
                case "html":
                    success = ViewAgent.Instance().SaveModelPage(id, Encoding.UTF8.GetString(file.ToArray()));
                    break;
                case "css":
                    success = ViewAgent.Instance().SaveModelStyle(id, Encoding.UTF8.GetString(file.ToArray()));
                    break;
                default:
                    success = ViewAgent.Instance().SaveModelResource(id, file);
                    break;
            }
            return this.GetResult(success);
        }

        /// <summary>
        /// 发布视图模型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task PublishModel([FromForm]int id)
        {
            return this.GetResult(ViewAgent.Instance().PublishModel(id));
        }

        /// <summary>
        /// 刪除模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task DeleteModel([FromForm]int id)
        {
            return this.GetResult(ViewAgent.Instance().DeleteModel(id));
        }

        /// <summary>
        /// 刪除模型中的資源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public Task DeleteModelResource([FromForm]int modelId, [FromForm]string name)
        {
            return this.GetResult(ViewAgent.Instance().DeleteModelResource(modelId, name));
        }

        #endregion

        #region =======  系统模板  ========

        /// <summary>
        /// 读取系统模板列表
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task TemplateList([FromForm]PlatformSource platform)
        {
            return this.GetResult(this.ShowResult(ViewAgent.Instance().GetTemplateList(platform), t => new
            {
                t.ID,
                t.Name,
                t.Platform,
                t.Preview
            }));
        }

        /// <summary>
        /// 读取系统模板（包含模型配置)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task GetTemplateInfo([FromForm]int id)
        {
            ViewTemplate template = ViewAgent.Instance().GetTemplateInfo(id) ?? new ViewTemplate()
            {
                Configs = new List<ViewTemplateConfig>()
            };
            return this.GetResult(new
            {
                template.ID,
                template.Platform,
                template.Name,
                template.Preview,
                Models = template.Configs.Select(t => t.ModelID)
            });
        }

        /// <summary>
        /// 保存系统模板
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task SaveTemplateInfo([FromForm]int id, [FromForm]string name, [FromForm]PlatformSource platform, [FromForm]string cover, [FromForm]string model)
        {
            ViewTemplate template = new ViewTemplate()
            {
                ID = id,
                Name = name,
                Platform = platform,
                Preview = cover
            };

            return this.GetResult(ViewAgent.Instance().SaveTemplateInfo(template, WebAgent.GetArray<int>(model)));
        }

        /// <summary>
        /// 删除系统模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task TemplateDelete([FromForm]int id)
        {
            return this.GetResult(ViewAgent.Instance().DeleteTemplate(id));
        }

        #endregion

        #region =======  模板  ========
        /// <summary>
        /// 读取商户模板列表
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task SiteTemplateList([FromForm]int siteid)
        {
            return this.GetResult(this.ShowResult(ViewAgent.Instance().GetSiteTemplateList(siteid), t => new
            {
                t.ID,
                t.Name,
                t.Platform,
                t.SiteID,
                t.Domain
            }));
        }

        /// <summary>
        /// 删除商户模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task SiteTemplateDelete([FromForm]int id)
        {
            return this.GetResult(ViewAgent.Instance().DeleteSiteTemplate(id));
        }

        /// <summary>
        /// 保存商户模板
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task SaveSiteTemplateInfo([FromForm]int id, [FromForm]string name, [FromForm]PlatformSource platform, [FromForm]string domain, [FromForm]int siteid, [FromForm]string model)
        {
            ViewSiteTemplate template = new ViewSiteTemplate()
            {
                ID = id,
                Name = name,
                Platform = platform,
                Domain = domain,
                SiteID = siteid
            };

            return this.GetResult(ViewAgent.Instance().SaveSiteTemplateInfo(template, WebAgent.GetArray<int>(model)));
        }

        /// <summary>
        /// 读取商户模板（包含模型配置)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task GetSiteTemplateInfo([FromForm]int id)
        {
            ViewSiteTemplate template = ViewAgent.Instance().GetSiteTemplateInfo(id) ?? new ViewSiteTemplate()
            {
                Configs = new List<ViewSiteConfig>()
            };
            return this.GetResult(new
            {
                template.ID,
                template.Platform,
                template.Name,
                template.Domain,
                template.SiteID,
                Models = template.Configs.Select(t => t.ModelID)
            });
        }
        #endregion
    }
}
