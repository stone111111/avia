using Aliyun.OSS;
using BW.Agent.Systems;
using BW.Cache.Sites;
using BW.Cache.Systems;
using BW.Common.Systems;
using BW.Common.Views;
using BW.Views;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SP.StudioCore.API;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Json;
using SP.StudioCore.Security;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BW.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 视图管理
    /// </summary>
    public sealed class ViewAgent : IViewAgent<ViewAgent>
    {
        /// <summary>
        /// 数据初始化
        /// 1、把视图类库的实现类全部生成数据
        /// </summary>
        public int Initialize()
        {
            Assembly assembly = typeof(ViewUtils).Assembly;
            Type[] types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(IViewBase)) && !t.IsAbstract).ToArray();
            foreach (Type type in types)
            {
                this.SaveViewSetting(type);
            }
            return types.Length;
        }

        /// <summary>
        /// 保存类库中的视图
        /// </summary>
        /// <param name="type"></param>
        private void SaveViewSetting(Type type)
        {
            PlatformSource? platform = ViewUtils.GetPlatform(type);
            if (platform == null) return;
            ViewSetting setting = new ViewSetting()
            {
                Name = type.Name,
                Platform = platform.Value,
                Code = type.FullName,
                Status = ViewSetting.ViewStatus.Normal
            };
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (setting.Exists(db, t => t.Code))
                {
                    setting = setting.Info(db, t => t.Code);
                }
                else
                {
                    setting.AddIdentity(db);
                }
                db.AddCallback(() =>
                {
                    ViewCaching.Instance().SaveViewID(setting.Code, setting.ID);
                });
                db.Commit();
            }
        }

        /// <summary>
        /// 获取系统的视图配置对象
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public ViewSetting GetViewSetting(int viewId)
        {
            return this.ReadDB.ReadInfo<ViewSetting>(t => t.ID == viewId);
        }

        #region ========  模型管理  ========

        /// <summary>
        /// 获取视图模型
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public new ViewModel GetModelInfo(int modelId)
        {
            return base.GetModelInfo(modelId);
        }

        /// <summary>
        /// 保存视图模型
        /// </summary>
        /// <param name="model"></param>
        public bool SaveModelInfo(ViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name)) return this.FaildMessage("请输入模型名字");
            if (string.IsNullOrEmpty(model.Path)) return this.FaildMessage("请输入模型路径");
            if (string.IsNullOrEmpty(model.Description)) return this.FaildMessage("请输入模型说明");
            if (string.IsNullOrEmpty(model.Preview)) return this.FaildMessage("请输入模型预览图");
            if (!this.ReadDB.Exists<ViewSetting>(t => t.ID == model.ViewID)) return this.FaildMessage("视图选择错误");

            ViewModel source = this.GetModelInfo(model.ID);
            using (DbExecutor db = NewExecutor())
            {
                if (source == null)
                {
                    model.AddIdentity(db);
                }
                else
                {
                    model.Update(db, t => t.Name, t => t.Description, t => t.Preview, t => t.Path, t => t.Resources);
                }
            }

            return true;

        }

        /// <summary>
        /// 保存模型的页面内容（还需要做语种转换存放到 view_Content 中）
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool SaveModelPage(int modelId, string page)
        {
            if (modelId == 0) return this.FaildMessage("模型编号错误");
            if (string.IsNullOrEmpty(page)) return this.FaildMessage("没有上传内容");
            using (DbExecutor db = NewExecutor())
            {
                return new ViewModel() { ID = modelId, Page = page }.Update(db, t => t.Page) == 1;
            }
        }

        /// <summary>
        /// 保存模型的样式内容
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        internal bool SaveModelStyle(int modelId, string style)
        {
            if (modelId == 0) return this.FaildMessage("模型编号错误");
            if (string.IsNullOrEmpty(style)) return this.FaildMessage("没有上传内容");
            using (DbExecutor db = NewExecutor())
            {
                return new ViewModel() { ID = modelId, Style = style }.Update(db, t => t.Style) == 1;
            }
        }

        /// <summary>
        /// 保存资源文件（存放于OSS上）
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="resourceFile"></param>
        /// <returns></returns>
        internal bool SaveModelResource(int modelId, IFormFile file)
        {
            byte[] data = file.ToArray();
            ViewModel.ResourceFile resource = new ViewModel.ResourceFile(file, data);
            if (modelId == 0) return this.FaildMessage("模型编号错误");
            if (string.IsNullOrEmpty(resource.Name)) return this.FaildMessage("没有上传内容");

            using (DbExecutor db = NewExecutor())
            {
                ViewModel model = new ViewModel() { ID = modelId }.Info(db);
                if (model == null) return this.FaildMessage("模型编号错误");

                if (model.ResourceFiles.ContainsKey(resource.Name))
                {
                    model.ResourceFiles[resource.Name] = resource;
                }
                else
                {
                    model.ResourceFiles.Add(resource.Name, resource);
                }
                model.Resources = JsonConvert.SerializeObject(model.ResourceFiles);

                // 上传文件至于OSS
                if (!OSSAgent.Upload(new OSSSetting(SettingAgent.Instance().GetSetting(SystemSetting.SettingType.CDNOSS)), resource.Path, data, new ObjectMetadata(), out string message))
                {
                    return this.FaildMessage(message);
                }

                return model.Update(db, t => t.Resources) == 1;
            }
        }

        /// <summary>
        /// 发布模型
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        internal bool PublishModel(int modelId)
        {
            ViewModel model = this.GetModelInfo(modelId);
            if (model == null) return this.FaildMessage("模型错误");

            Language[] languages = SettingAgent.Instance().GetSetting(SystemSetting.SettingType.Language).Split(',').Select(t => t.ToEnum<Language>()).ToArray();
            if (languages.Length == 0) return this.FaildMessage("没有配置语言包");

            string translateUrl = SettingAgent.Instance().GetSetting(SystemSetting.SettingType.Translate);
            if (string.IsNullOrEmpty(translateUrl)) return this.FaildMessage("没有配置语言包接口地址");

            //#1 找出样式文件中需要替换的资源路径
            model.Style = Regex.Replace(model.Style, @"url\((?<Resource>[^\)]+?)\)", t =>
           {
               string resource = t.Groups["Resource"].Value;
               if (model.ResourceFiles.ContainsKey(resource))
               {
                   return $"url({model.ResourceFiles[resource]})";
               }
               return t.Value;
           });

            using (DbExecutor db = NewExecutor())
            {
                model.Update(db, t => t.Style);
            }

            //#2 发布页面的各个语言版本
            {
                Regex regex = new Regex(@"~.+?~", RegexOptions.Multiline);
                //#2.1 找出单词
                Dictionary<string, bool> words = new Dictionary<string, bool>();
                foreach (Match match in regex.Matches(model.Page))
                {
                    string word = match.Value;
                    if (!words.ContainsKey(word)) words.Add(word, true);
                }
                //#2.2 发送到翻译API，获得语言包
                Dictionary<string, Dictionary<Language, string>> dic = Translate.Get(words.Select(t => t.Key).ToArray(), translateUrl, languages);

                foreach (Language language in languages)
                {
                    int total = 0;
                    int count = 0;
                    string content = regex.Replace(model.Page, t =>
                    {
                        total++;
                        if (dic.ContainsKey(t.Value) && dic[t.Value].ContainsKey(language))
                        {
                            count++;
                            return dic[t.Value][language];
                        }
                        return t.Value.Replace("~", string.Empty);
                    });
                    string path = $"html/{ Encryption.toMD5Short(Encryption.toMD5(content)) }.html";
                    if (!OSSAgent.Upload(new OSSSetting(SettingAgent.Instance().GetSetting(SystemSetting.SettingType.CDNOSS)),
                        path, Encoding.UTF8.GetBytes(content), new ObjectMetadata()
                        {
                            ContentType = "text/html",
                            ContentEncoding = "utf-8",
                            ContentDisposition = "inline",
                            CacheControl = "only-if-cached",
                            ExpirationTime = DateTime.Now.AddYears(1),
                        }, out string message))
                    {
                        return this.FaildMessage(message);
                    }
                    ViewContent viewContent = new ViewContent()
                    {
                        Language = language,
                        ModelID = modelId,
                        Path = path,
                        Translate = total == 0 ? 1M : count / (decimal)total
                    };
                    this.SaveModelContent(viewContent);
                }
            }

            return true;
        }

        /// <summary>
        /// 刪除模型（此方法還需要同步刪除模板中對於該模型的引用）
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        internal bool DeleteModel(int modelId)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                db.Delete<ViewContent>(t => t.ModelID == modelId);
                db.Delete<ViewModel>(t => t.ID == modelId);

                db.Commit();
            }
            return true;
        }

        /// <summary>
        /// 刪除模型中的資源
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool DeleteModelResource(int modelId, string name)
        {
            ViewModel model = this.GetModelInfo(modelId);
            if (model == null) return this.FaildMessage("編號錯誤");

            if (model.ResourceFiles.ContainsKey(name))
            {
                model.ResourceFiles.Remove(name);
                model.Resources = model.ResourceFiles.ToJson();
                using (DbExecutor db = NewExecutor())
                {
                    model.Update(db, t => t.Resources);
                }
                return true;
            }
            return this.FaildMessage("資源不存在");
        }

        /// <summary>
        /// 保存模型内容进入路径
        /// </summary>
        /// <param name="content"></param>
        private void SaveModelContent(ViewContent content)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (content.Exists(db))
                {
                    content.Update(db, t => t.Path, t => t.Translate);
                }
                else
                {
                    content.Add(db);
                }
                db.Commit();
            }
        }

        #endregion

        #region ========  系统模板管理  ========

        /// <summary>
        /// 获取系统模板（包括视图配置信息）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public new ViewTemplate GetTemplateInfo(int templateId)
        {
            return base.GetTemplateInfo(templateId);
        }

        /// <summary>
        /// 保存系统模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool SaveTemplateInfo(ViewTemplate template, int[] models)
        {
            if (string.IsNullOrEmpty(template.Preview)) return this.FaildMessage("请上传预览图");
            if (string.IsNullOrEmpty(template.Name)) return this.FaildMessage("请输入模板名称");

            bool isNew = template.ID == 0;
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (template.ID == 0)
                {
                    template.AddIdentity(db);
                }
                else
                {
                    template.Update(db, t => t.Name, t => t.Preview);
                }

                //# 得到当前平台下所有的视图
                List<ViewSetting> views = db.ReadList<ViewSetting>(t => t.Platform == template.Platform);
                List<ViewModel> modelList = new List<ViewModel>();
                foreach (int modelId in models)
                {
                    ViewModel model = db.ReadInfo<ViewModel>(t => t.ID == modelId);
                    if (model == null)
                    {
                        db.Rollback();
                        return this.FaildMessage($"模型ID{modelId}不存在");
                    }
                    ViewTemplateConfig config = isNew ? null :
                        config = db.ReadInfo<ViewTemplateConfig>(t => t.TemplateID == template.ID && t.ViewID == model.ViewID);
                    if (config != null && config.ModelID != modelId)
                    {
                        config.ModelID = modelId;
                        config.Update(db, t => t.ModelID);
                    }
                    else if (config == null)
                    {
                        config = new ViewTemplateConfig()
                        {
                            TemplateID = template.ID,
                            ViewID = model.ViewID,
                            ModelID = model.ID
                        };
                        config.Add(db);
                    }
                    modelList.Add(model);
                }

                ViewSetting view = views.FirstOrDefault(t => !modelList.Any(p => p.ViewID == t.ID));
                if (view != null)
                {
                    db.Rollback();
                    return this.FaildMessage($"视图{view.Name}未选则模型");
                }

                db.Commit();
            }

            return this.AccountInfo.Log(LogType.View, $"保存系统模板 {template.Platform}/{template.Name}");
        }

        /// <summary>
        /// 删除系统模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool DeleteTemplate(int templateId)
        {
            ViewTemplate template = this.GetTemplateInfo(templateId);
            if (template == null) return this.FaildMessage("");
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                template.Configs.ForEach(config =>
                {
                    config.Delete(db);
                });
                template.Delete(db);
                db.Commit();
            }
            return this.AccountInfo.Log(LogType.View, $"删除系统模板 {template.Platform}/{template.Name}");
        }

        #endregion

        #region ========  商户模板管理  ========

        /// <summary>
        /// 复制系统模板到商户模板
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public new int CopySystemTemplate(int siteId, int templateId)
        {
            return base.CopySystemTemplate(siteId, templateId);
        }

        /// <summary>
        /// 删除商户模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool DeleteSiteTemplate(int templateId)
        {
            ViewSiteTemplate template = this.GetSiteTemplateInfo(templateId);
            if (template == null) return this.FaildMessage("");
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                template.Configs.ForEach(config =>
                {
                    config.Delete(db);
                });
                template.Delete(db);
                db.Commit();
            }
            return this.AccountInfo.Log(LogType.View, $"删除商户模板 {template.Platform}/{template.Name}");
        }

        /// <summary>
        /// 获取商户模板（包括视图配置信息）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public new ViewSiteTemplate GetSiteTemplateInfo(int templateId)
        {
            return base.GetSiteTemplateInfo(templateId);
        }

        /// <summary>
        /// 保存商户模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool SaveSiteTemplateInfo(ViewSiteTemplate template, int[] models)
        {
            if (string.IsNullOrEmpty(template.Name)) return this.FaildMessage("请输入模板名称");
            if (!string.IsNullOrEmpty(template.Domain) && !isDomain(template.Domain)) return this.FaildMessage("域名错误，请重新输入域名");

            bool isNew = template.ID == 0;
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (template.ID == 0)
                {
                    template.AddIdentity(db);
                }
                else
                {
                    template.Update(db, t => t.Name, t => t.Platform, t => t.Domain);
                }

                //# 得到当前平台下所有的视图
                List<ViewSetting> views = db.ReadList<ViewSetting>(t => t.Platform == template.Platform);
                List<ViewModel> modelList = new List<ViewModel>();
                foreach (int modelId in models)
                {
                    ViewModel model = db.ReadInfo<ViewModel>(t => t.ID == modelId);
                    if (model == null)
                    {
                        db.Rollback();
                        return this.FaildMessage($"模型ID{modelId}不存在");
                    }
                    ViewSiteConfig config = isNew ? null :
                        config = db.ReadInfo<ViewSiteConfig>(t => t.TemplateID == template.ID && t.ViewID == model.ViewID && t.SiteID == template.SiteID);
                    if (config != null && config.ModelID != modelId)
                    {
                        config.ModelID = modelId;
                        config.Update(db, t => t.ModelID);
                    }
                    else if (config == null)
                    {
                        config = new ViewSiteConfig()
                        {
                            TemplateID = template.ID,
                            ViewID = model.ViewID,
                            ModelID = model.ID,
                            SiteID = template.SiteID
                        };
                        config.Add(db);
                    }
                    modelList.Add(model);
                }

                ViewSetting view = views.FirstOrDefault(t => !modelList.Any(p => p.ViewID == t.ID));
                if (view != null)
                {
                    db.Rollback();
                    return this.FaildMessage($"视图{view.Name}未选则模型");
                }

                db.AddCallback(() =>
                {
                    SiteCaching.Instance().RemoveSiteInfo(template.SiteID);
                });

                db.Commit();
            }

            return this.AccountInfo.Log(LogType.View, $"保存系统模板 {template.Platform}/{template.Name}");
        }

        #endregion

        #region ========  商户视图配置  ========

        /// <summary>
        /// 获取商户对于视图的配置（数据库读取)
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public ViewSiteConfig GetSiteViewConfig(int configId)
        {
            return this.ReadDB.ReadInfo<ViewSiteConfig>(t => t.ID == configId);
        }

        /// <summary>
        /// 保存商户对于视图的配置
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public bool SaveSiteViewConfig(int configId, string setting)
        {
            ViewSiteConfig config = this.GetSiteViewConfig(configId);
            if (config == null) return this.FaildMessage("配置项目错误");
            // 验证setting内容是否正确
            ViewSetting view = this.GetViewSetting(config.ViewID);
            try
            {
                ViewUtils.CreateInstance(view.Code, setting);
            }
            catch (Exception ex)
            {
                return this.FaildMessage(ex.Message);
            }
            config.Setting = setting;
            this.WriteDB.Update<ViewSiteConfig, string>(t => t.Setting, setting, t => t.ID == configId);
            ViewCaching.Instance().SaveSiteConfig(config);
            return this.AccountInfo.Log(LogType.Site, $"修改商户{config.SiteID}视图配置参数,ID={configId},View={view.Code}");
        }

        #endregion
    }
}
