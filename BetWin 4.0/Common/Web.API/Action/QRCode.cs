using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Types;

namespace Web.API.Action
{
    /// <summary>
    /// 生成二维码（输出图片流对象）
    /// 需要安装 yum install -y libgdiplus
    /// </summary>
    public class QRCode : IAction
    {
        public QRCode(HttpContext context) : base(context)
        {
        }

        public override Result Invote()
        {
            string content = HttpUtility.UrlDecode(context.Request.Query["content"]);
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                qrEncoder.TryEncode(content, out QrCode qrCode);
                int moduleSize = 9;
                GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                using (MemoryStream ms = new MemoryStream())
                {
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                    return new Result(ContentType.PNG, ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }
    }
}
