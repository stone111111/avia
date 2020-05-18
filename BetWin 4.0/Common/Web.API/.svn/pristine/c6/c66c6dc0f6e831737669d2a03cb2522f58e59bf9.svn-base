using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using Web.API.Properties;

namespace Web.API.Action
{
    /// <summary>
    /// 图片验证码
    /// </summary>
    public class ValidCode : IAction
    {
        public ValidCode(HttpContext context) : base(context)
        {
        }

        public ValidCode(string[] args) : base(args)
        {
        }

        public override Result Invote()
        {
            string key = this.GetKey(this.context.QS("k"));
            if (string.IsNullOrEmpty(key)) return new Result(HttpStatusCode.BadRequest, 0, null, null);
            Result result = default;
            switch (this.context.Request.Method)
            {
                case "GET":
                    result = this.ShowCode(key, 80, 30);
                    break;
                case "POST":
                    result = this.CheckCode(key, this.context.QF("v"));
                    break;
            }
            return result;
        }

        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <param name="key"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Result ShowCode(string key, int width, int height)
        {
            string random = WebAgent.GetRandom(0, 9999).ToString().PadLeft(4, '0');
            MemoryUtils.Set(key, random, TimeSpan.FromMinutes(5));
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                Bitmap bg = (Bitmap)new ResourceManager(typeof(Resources)).GetObject("validcode_bg");
                Rectangle rectangle = new Rectangle(WebAgent.GetRandom(0, bg.Width - width),
                    WebAgent.GetRandom(0, bg.Height - height),
                    width, height);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.DrawImage(bg, new Rectangle(0, 0, width, height),
                        rectangle, GraphicsUnit.Pixel);
                    for (int i = 0; i < random.Length; i++)
                    {
                        Bitmap code = (Bitmap)new ResourceManager(typeof(Resources)).GetObject($"validcode_{random[i]}");
                        Rectangle codeRectangle = new Rectangle(width / random.Length * i + WebAgent.GetRandom(-5, 5), WebAgent.GetRandom(-5, 5), width / 4, height);
                        g.DrawImage(code, codeRectangle,
                            new Rectangle(0, 0, code.Width, code.Height),
                            GraphicsUnit.Pixel);
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return new Result(ContentType.PNG, ms.ToArray());
                }
            }
        }

        private Result CheckCode(string key, string value)
        {
            string code = MemoryUtils.Get<string>(key);
            Result result = new Result(!string.IsNullOrEmpty(code) && code == value);
            if (result) MemoryUtils.Remove(key);
            return result;
        }

        private string GetKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return $"{this.GetType().Name}:{key}";
        }
    }
}
