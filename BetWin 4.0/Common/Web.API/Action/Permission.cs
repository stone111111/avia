using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using SP.StudioCore.Xml;

namespace Web.API.Action
{
    /// <summary>
    /// 权限生成
    /// </summary>
    public class Permission : IAction
    {
        public Permission(HttpContext context) : base(context)
        {
        }

        public override Result Invote()
        {
            if (this.context.Request.Method != "POST" || this.context.Request.Form.Files.Count == 0) throw new Exception("未匹配");
            var file = this.context.Request.Form.Files[0];
            Result result;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] data = ms.ToArray();
                switch (this.context.Request.Headers["action"])
                {
                    case "XML":
                        result = new Result(ContentType.XML, this.Xml(Encoding.UTF8.GetString(data)));
                        break;
                    case "CS":
                        result = new Result(ContentType.TEXT, this.Build(Encoding.UTF8.GetString(data), this.context.Request.Headers["name"]));
                        break;
                    default:
                        result = new Result(false, "未指定动作");
                        break;
                }
            }
            return result;
        }

        private string Xml(string xml)
        {
            try
            {
                if (xml.IndexOf("<root") > 0) xml = xml.Substring(xml.IndexOf("<root"));
                XElement root = XElement.Parse(xml);
                this.Xml(root);
                return root.ToString();
            }
            catch 
            {
                throw new Exception(xml);
            }
        }

        private void Xml(XElement element)
        {
            string id = element.GetAttributeValue("ID");
            if (string.IsNullOrEmpty(id)) element.SetAttributeValue("ID", Guid.NewGuid().ToString("N"));
            foreach (XElement item in element.Elements())
            {
                this.Xml(item);
            }
        }


        private string Build(string xml, string name)
        {
            if (xml.IndexOf("<root") > 0) xml = xml.Substring(xml.IndexOf("<root"));
            XElement root = XElement.Parse(xml);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;")
            .AppendLine(@"using System.Collections.Generic;")
            .AppendLine("namespace BW{")
            .AppendFormat("public static class {0}", name)
            .AppendLine("{");

            Dictionary<string, string> _name = new Dictionary<string, string>();
            sb.AppendLine(@"public static readonly Dictionary<string, string> NAME = new Dictionary<string, string>(){ ${NAME} };");

            foreach (XElement item in root.Elements())
            {
                Build(item, sb, _name);
            }
            sb.AppendLine(@"}}");

            return sb.ToString().Replace("${NAME}", string.Join(",\n", _name.Select(t => string.Concat("{\"", t.Key, "\",\"", t.Value, "\"}"))));
        }


        void Build(XElement element, StringBuilder sb, Dictionary<string, string> _name)
        {
            string name = element.Attribute("name").Value;
            string value = element.Attribute("ID").Value;
            string tag = element.Name.ToString();

            switch (tag)
            {
                case "menu":
                    sb.AppendLine(" public static class " + name + "   { ");
                    sb.AppendFormat("public const string Value = \"{0}\";", value).AppendLine();
                    foreach (XElement item in element.Elements())
                    {
                        Build(item, sb, _name);
                    }
                    sb.AppendLine("}");
                    break;
                case "action":
                    sb.AppendFormat("public const string {0} = \"{1}\";", name, value);
                    break;
            }

            List<string> permissionName = new List<string>();
            while (element != null && element.Attribute("name") != null)
            {
                permissionName.Add(element.Attribute("name").Value);
                element = element.Parent;
            }
            if (!_name.ContainsKey(value)) _name.Add(value, string.Join(".", permissionName));
        }

    }
}
