using BW.Agent.Sites;
using BW.Agent.Systems;
using BW.Cache.Systems;
using BW.Common.Sites;
using BW.Common.Systems;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Web.System.Properties;
using System.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.API;
using static BW.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Sites
{
    /// <summary>
    /// 商户证书
    /// </summary>
    public sealed class CertAgent : ICertAgent<CertAgent>
    {

        #region ========  商户证书  ========

        /// <summary>
        /// 添加SSL证书
        /// </summary>
        /// <param name="cert"></param>
        /// <returns></returns>
        public bool SaveCertInfo(SiteDomainCert cert)
        {
            if (cert.SiteID == 0) return this.FaildMessage("没有指定站点");
            if (string.IsNullOrEmpty(cert.PEM)) return this.FaildMessage("未选择证书");
            if (string.IsNullOrEmpty(cert.KEY)) return this.FaildMessage("未选择密钥");

            CertInfo info = HttpsHelper.GetFirstCertInfo(cert.PEM);
            if (!info.Success) return this.FaildMessage(info.Message);
            cert.Name = info.Message;
            cert.Domain = string.Join(",", info.Domain);
            cert.Expire = info.ExpireAt;
            cert.CreateAt = DateTime.Now;

            if (this.ReadDB.Exists<SiteDomainCert>(t => t.SiteID == cert.SiteID && t.ID != cert.ID && t.Name == cert.Name)) return this.FaildMessage("已存在同名的证书");
            bool success = false;

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (cert.ID == 0)
                {
                    success = cert.Add(db);
                }
                else
                {
                    success = cert.Update(db) == 1;
                }
                db.Commit();
            }
            return success && AccountInfo.Log(LogType.Site, string.Format("设定域名证书 {0}", cert.Name));
        }

        /// <summary>
        /// 获取证书内容
        /// </summary>
        /// <param name="certCode"></param>
        /// <returns></returns>
        public SiteDomainCert GetCertInfo(int certId)
        {
            return this.ReadDB.ReadInfo<SiteDomainCert>(t => t.ID == certId);
        }

        /// <summary>
        /// 删除SSL证书
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public bool DeleteCertInfo(int certId)
        {
            SiteDomainCert cert = this.GetCertInfo(certId);
            if (cert == null) return this.FaildMessage("编号错误");
            return this.WriteDB.Delete(cert) &&
                 AccountInfo.Log(LogType.Site, string.Format("删除站点{0} 证书{1}", cert.SiteID, cert.Name));
        }

        #endregion
    }
}
