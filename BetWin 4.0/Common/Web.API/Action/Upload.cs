using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SP.StudioCore.API;
using SP.StudioCore.Http;
using SP.StudioCore.IO;
using SP.StudioCore.Model;
using SP.StudioCore.Net;
using SP.StudioCore.Security;

namespace Web.API.Action
{
    /// <summary>
    /// 图片上传接口
    /// </summary>
    public class Upload : IAction
    {
        /// <summary>
        /// 上传文件要保存的路径
        /// </summary>
        private static readonly string _path;

        /// <summary>
        /// 读取配置文件
        /// </summary>
        static Upload()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

            _path = config["upload:path"];
        }

        public Upload(HttpContext context) : base(context)
        {
        }


        /// <summary>
        /// 可以支持的文件类型
        /// </summary>
        private readonly string[] fileType = new string[] { "jpeg", "png", "gif", "bmp", "webp", "svg", "mp3", "css", "js", "html" };

        public override Result Invote()
        {
            if (string.IsNullOrEmpty(_path)) return new Result(false, "not configured file save path");

            // 是否是需要向前兼容的版本（旧版本如果出错则返回空值）
            bool isBackward = context.Request.Path.StartsWithSegments("/imageupload.ashx");

            string uploadFolder = null;
            string fileFolder = null;
            try
            {
                // 自定义的后缀名
                string type = !this.context.Request.Headers.ContainsKey("x-type") ? null : this.context.Request.Headers["x-type"].ToString();

                byte[] data = this.context.GetData();
                if (data == null || data.Length < 2) return isBackward ? new Result("") : new Result(false, "Content is Empty");
                string fileName = Encryption.toMD5(data).Substring(0, 16).ToLower();
                // 相对路径
                fileFolder = "upload/" + DateTime.Now.ToString("yyyyMM");
                // 绝对路径
                uploadFolder = _path + DateTime.Now.ToString("yyyyMM");

                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);
                string contentType = FileAgent.GetContentType(data);
                if (string.IsNullOrEmpty(contentType) && string.IsNullOrEmpty(type)) return isBackward ? new Result("") : new Result(false, "文件类型未知");
                string ext = type ?? Regex.Match(contentType, @"/(?<Type>\w+)$").Groups["Type"].Value;
                if (!fileType.Contains(ext)) return isBackward ? new Result("") : new Result(false, $"文件类型{ext}不支持");

                // 需要保存的绝对路径
                string filePath = $"{uploadFolder}/{ fileName}.{ ext}";

                // 对外输出的相对路径
                fileName = $"{fileFolder}/{fileName}.{ext}";

                if (!OSSAgent.Upload(_ossSetting, fileName, data, null, out string message))
                {
                    return new Result(false, message);
                }

                if (!File.Exists(filePath))
                {
                    File.WriteAllBytes(filePath, data);
                }
                if (isBackward)
                {
                    return new Result(ContentType.TEXT, $"/{fileName}");
                }
                else
                {
                    return new Result(true, "Success", new
                    {
                        fileName = $"/{fileName}"
                    });
                }
            }
            catch (Exception ex)
            {
                if (isBackward)
                {
                    return new Result("");
                }
                else
                {
                    return new Result(false, $"{ex.Message}\nfileFolder:{fileFolder}\nuploadFolder:{uploadFolder}");
                }
            }
        }
    }
}
