using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SP.StudioCore.API;
using SP.StudioCore.Http;
using SP.StudioCore.IO;
using SP.StudioCore.Json;
using SP.StudioCore.Mobile.Android.ApkReader;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using SP.StudioCore.Text;
using SP.StudioCore.Web;

namespace Web.API.Action
{
    /// <summary>
    /// APP的解析和生成下载页面
    /// </summary>
    public class APP : IAction
    {
        public APP(HttpContext context) : base(context)
        {
        }

        public override Result Invote()
        {
            Result result = default;
            switch (this.context.Request.Method)
            {
                case "POST":
                    if (this.context.Request.Form.Files.Count != 1)
                    {
                        return new Result(false, "No File Uploaded");
                    }
                    IFormFile file = this.context.Request.Form.Files[0];
                    string fileType = this.context.QS("type") ?? file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
                    switch (fileType)
                    {
                        case "apk":
                            result = this.SaveApk(file);
                            break;
                        case "ipa":
                            break;
                        default:
                            result = new Result(false, $"not support {fileType}");
                            break;
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 解析并且保存安卓文件
        /// </summary>
        /// <returns></returns>
        private Result SaveApk(IFormFile file)
        {
            string localFile = @$"{Directory.GetCurrentDirectory()}/temp/{DateTime.Now.Ticks}.apk";
            using (FileStream fileStream = new FileStream(localFile, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string fileMD5 = FileAgent.GetMD5(localFile).Substring(0, 4);

            byte[] AndroidManifest, resources, iconData;
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(localFile))
                {
                    ZipArchiveEntry entry = zip.GetEntry("AndroidManifest.xml");
                    AndroidManifest = new byte[entry.Length];
                    entry.Open().Read(AndroidManifest, 0, AndroidManifest.Length);

                    ZipArchiveEntry ascr = zip.GetEntry("resources.arsc");
                    resources = new byte[ascr.Length];
                    ascr.Open().Read(resources, 0, resources.Length);

                    ApkReader apkReader = new ApkReader();
                    ApkInfo info = apkReader.extractInfo(AndroidManifest, resources);

                    string iconFile = info.iconFileName.Where(t => t.EndsWith(".png")).LastOrDefault();
                    ZipArchiveEntry iconEntry = zip.GetEntry(iconFile);
                    iconData = new byte[iconEntry.Length];
                    iconEntry.Open().Read(iconData, 0, iconData.Length);

                    // 上传文件至OSS
                    string version = this.GetVersion(info.versionName);
                    string fileName = $"app/{info.label.ToPinYinAbbr().ToLower()}_{version}_{fileMD5}.apk";

                    string iconPath = $"app/{Encryption.toMD5(iconData).Substring(0, 16).ToLower()}.png";

                    string ossMessage;
                    if (!OSSAgent.Upload(_ossSetting, fileName, localFile, out ossMessage))
                    {
                        return new Result(false, ossMessage, new
                        {
                            fileName,
                            localFile
                        });
                    }
                    if (!OSSAgent.Upload(_ossSetting, iconPath, iconData, null, out ossMessage))
                    {
                        return new Result(false, ossMessage);
                    }
                    string configFile = $"app/{DateTime.Now.ToString("yyyyMMHHmmss")}.json";
                    string json = new
                    {
                        Name = info.label,
                        File = fileName,
                        Version = version,
                        Icon = iconPath,
                        Config = configFile,
                        UpdateAt = DateTime.Now
                    }.ToJson();
                    if (!OSSAgent.Upload(_ossSetting, configFile, Encoding.UTF8.GetBytes(json), null, out ossMessage))
                    {
                        return new Result(false, ossMessage);
                    }
                    return new Result(true, fileName, json);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
            finally
            {
                File.Delete(localFile);
            }
        }


        private string GetVersion(string version)
        {
            Regex regex = new Regex(@"^[\d\.]+");
            if (!regex.IsMatch(version)) return "1.0.0";
            return regex.Match(version).Value;
        }
    }
}
