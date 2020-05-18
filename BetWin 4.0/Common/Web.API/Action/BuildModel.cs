using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Types;
using Web.API.Properties;

namespace Web.API.Action
{
    /// <summary>
    /// 实体类的创建
    /// </summary>
    public partial class BuildModel : IAction
    {
        public BuildModel(HttpContext context) : base(context)
        {
        }

        public override Result Invote()
        {
            if (this.context.Request.Method != "POST" || this.context.Request.Form.Files.Count == 0)
            {
                return new Result(ContentType.TEXT, Resources.DataQuery);
            }
            IFormFile query = this.context.Request.Form.Files["query"];
            IFormFile code = this.context.Request.Form.Files["code"];

            if (query == null) throw new Exception("query 不存在");
            if (code == null) throw new Exception("code 不存在");

            TableInfo table = new TableInfo();
            using (MemoryStream ms = new MemoryStream())
            {
                query.CopyTo(ms);
                byte[] data = ms.ToArray();
                string queryString = Encoding.UTF8.GetString(data);

                List<ColumnInfo> columns = new List<ColumnInfo>();
                foreach (string line in queryString.Split('\n'))
                {
                    if (line.StartsWith("TABLE|"))
                    {
                        table = new TableInfo(line);
                    }
                    if (line.StartsWith("COLUMN|"))
                    {
                        table.AddColumn(new ColumnInfo(line));
                    }
                }
            }
            string codeContent = null;
            using (MemoryStream ms = new MemoryStream())
            {
                code.CopyTo(ms);
                byte[] data = ms.ToArray();
                codeContent = Encoding.UTF8.GetString(data);
            }
            return new Result(ContentType.TEXT, new CodeInfo(codeContent, table).ToString());
        }

        private static readonly Dictionary<string, string> _columnType = new Dictionary<string, string>()
        {
            {"BIGINT","long"},
            {"BIT","bool"},
            {"CHAR","string"},
            {"DATE","DateTime"},
            {"DATETIME","DateTime"},
            {"INT","int"},
            {"MONEY","decimal"},
            {"NCHAR","string"},
            {"NUMERIC","decimal"},
            {"NVARCHAR","string"},
            {"SMALLDATETIME","DateTime"},
            {"SMALLINT","short"},
            {"TINYINT","byte"},
            {"UNIQUEIDENTIFIER","Guid"},
            {"VARCHAR","string"},
            {"XML","string"}
         };

        /// <summary>
        /// 代码文件
        /// </summary>
        struct CodeInfo
        {
            public CodeInfo(string content, TableInfo table)
            {
                this.Table = table;

                Regex regex = new Regex(@"^namespace[ ]+(?<Name>[\w\.]+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                this.NameSpace = regex.Match(content).Groups["Name"].Value;

                this.Using = new List<string>()
                {
                    "using System;",
                    "using System.Data;",
                    "using System.ComponentModel.DataAnnotations;",
                    "using System.ComponentModel.DataAnnotations.Schema;"
                };
                foreach (Match match in Regex.Matches(content, @"using (?<Space>.+?);"))
                {
                    string usingName = match.Value;
                    if (!this.Using.Contains(usingName))
                    {
                        this.Using.Add(usingName);
                    }
                }

                Regex codeRegex = new Regex(@"#region  ========  扩展方法  ========.+?#endregion", RegexOptions.Singleline);
                if (codeRegex.IsMatch(content))
                {
                    this.CodeContent = codeRegex.Match(content).Value;
                }
                else
                {
                    this.CodeContent = "        #region  ========  扩展方法  ========\n\r\n\r        #endregion";
                }
            }

            /// <summary>
            /// 命名空间
            /// </summary>
            private string NameSpace;

            /// <summary>
            /// 已经引用的命名空间
            /// </summary>
            private List<string> Using;

            private TableInfo Table;

            /// <summary>
            /// 自己写的扩展方法
            /// </summary>
            private string CodeContent;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join("\n", this.Using))
                    .AppendLine();

                sb.AppendLine($"namespace {this.NameSpace}")
                    .AppendLine("{")
                    .AppendLine(this.Table.ToString(this.CodeContent))
                    .AppendLine("}");

                return sb.ToString();
            }
        }


        struct TableInfo
        {
            public TableInfo(string line) : this()
            {
                string[] str = line.Split('|');
                if (str.Length != 3) return;
                this.TableName = str[1].Trim();
                string tableDesc = str[2].Trim();
                if (!string.IsNullOrEmpty(tableDesc))
                {
                    Regex regex = new Regex(@"\[(?<Name>[^\]]+)\]");
                    if (regex.IsMatch(tableDesc))
                    {
                        this.ClassName = regex.Match(tableDesc).Groups["Name"].Value;
                    }
                    this.Description = Regex.Replace(tableDesc, @"\[.+\]|\n|\r", string.Empty);
                }
                if (string.IsNullOrEmpty(this.ClassName)) this.ClassName = this.TableName;
                this.columns = new List<ColumnInfo>();
            }

            /// <summary>
            /// 数据库内表的名字
            /// </summary>
            public string TableName;

            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName;

            /// <summary>
            /// 表备注
            /// </summary>
            public string Description;

            private List<ColumnInfo> columns;

            public void AddColumn(ColumnInfo column)
            {
                this.columns.Add(column);
            }

            public string ToString(string codeContent)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Description))
                {
                    sb.AppendLine($@"    /// <summary>
    /// {this.Description}
    /// </summary>");
                }
                sb.AppendLine($"    [Table(\"{ this.TableName }\")]");
                sb.AppendLine($"    public partial class {this.ClassName}")
                    .AppendLine("    {")
                    .AppendLine()
                    .AppendLine("        #region  ========  構造函數  ========")
                    .AppendLine("        public " + this.ClassName + "() { }")
                    .AppendLine()
                    .AppendLine(this.Reader())
                    .AppendLine()
                    .AppendLine(this.DataRow())
                    .AppendLine("        #endregion")
                    .AppendLine()
                    .AppendLine("        #region  ========  数据库字段  ========");
                foreach (ColumnInfo column in this.columns)
                {
                    sb.AppendLine()
                        .AppendLine(column.ToString());
                }
                sb.AppendLine("        #endregion");

                if (!string.IsNullOrEmpty(codeContent))
                {
                    sb.AppendLine()
                        .AppendLine()
                        .AppendLine(codeContent)
                        .AppendLine();
                }

                sb.AppendLine("    }");

                return sb.ToString();
            }

            /// <summary>
            /// IDataReader 構造函數
            /// </summary>
            /// <returns></returns>
            private string Reader()
            {
                StringBuilder sb = new StringBuilder()
                    .AppendLine("        public " + this.ClassName + "(IDataReader reader)")
                    .AppendLine("        {")
                    .AppendLine("            for (int i = 0; i < reader.FieldCount; i++)")
                    .AppendLine("            {")
                    .AppendLine("                switch (reader.GetName(i))")
                    .AppendLine("                {");
                foreach (ColumnInfo column in this.columns)
                {
                    sb.AppendLine("                    case \"" + column.ColumnName + "\":")
                        .AppendLine("                        this." + column.Name + " = (" + column.Type + ")reader[i];")
                        .AppendLine("                        break;");
                }
                sb.AppendLine("                }")
                    .AppendLine("            }")
                    .AppendLine("        }");

                return sb.ToString();
            }

            /// <summary>
            /// DataRow 構造函數
            /// </summary>
            /// <returns></returns>
            private string DataRow()
            {
                StringBuilder sb = new StringBuilder()
                    .AppendLine("        public " + this.ClassName + "(DataRow dr)")
                    .AppendLine("        {")
                    .AppendLine("            for (int i = 0; i < dr.Table.Columns.Count; i++)")
                    .AppendLine("            {")
                    .AppendLine("                switch (dr.Table.Columns[i].ColumnName)")
                    .AppendLine("                {");
                foreach (ColumnInfo column in this.columns)
                {
                    sb.AppendLine("                    case \"" + column.ColumnName + "\":")
                        .AppendLine("                        this." + column.Name + " = (" + column.Type + ")dr[i];")
                        .AppendLine("                        break;");
                }
                sb.AppendLine("                }")
                    .AppendLine("            }")
                    .AppendLine("        }");

                return sb.ToString();
            }
        }

        /// <summary>
        /// 字段信息
        /// </summary>
        struct ColumnInfo
        {
            /// <summary>
            /// COLUMN|1|GameID|[ID]游戏编号|1|1|int
            /// </summary>
            /// <param name="line"></param>
            public ColumnInfo(string line) : this()
            {
                string[] data = line.Split('|');
                this.ColID = data[1].Trim().GetValue<int>();
                this.ColumnName = data[2].Trim();
                this.Description = data[3].Trim();
                this.Identity = data[4].Trim().GetValue<bool>();
                this.Key = data[5].Trim().GetValue<bool>();
                this.ColumnType = data[6].Trim().ToUpper();

                if (!string.IsNullOrEmpty(this.Description))
                {
                    Regex regex = new Regex(@"\[(?<Extend>[^\]]+)\]");
                    foreach (Match match in regex.Matches(this.Description))
                    {
                        string extend = match.Groups["Extend"].Value;
                        if (Regex.IsMatch(extend, @"^\w+$", RegexOptions.IgnoreCase))
                        {
                            this.Name = extend;
                            continue;
                        }
                        else if (extend.Contains("="))
                        {
                            string type = extend.Substring(0, extend.IndexOf('='));
                            string value = extend.Substring(extend.IndexOf('=') + 1);
                            switch (type)
                            {
                                case "Type":
                                    this.Type = value;
                                    break;
                            }
                        }
                    }
                    this.Description = Regex.Replace(this.Description, @"\[.+\]", string.Empty);
                    this.Description = Regex.Replace(this.Description, "\n|\r", "\t");
                }

                if (string.IsNullOrEmpty(Name)) this.Name = this.ColumnName;

                if (string.IsNullOrEmpty(this.Type))
                {
                    this.Type = _columnType.Get(this.ColumnType, this.ColumnType);
                }
            }

            /// <summary>
            /// 字段编号
            /// </summary>
            public int ColID;

            /// <summary>
            /// 数据库字段名
            /// </summary>
            public string ColumnName;

            /// <summary>
            /// 数据库内的类型
            /// </summary>
            public string ColumnType;

            /// <summary>
            /// 自定义的名字
            /// </summary>
            public string Name;

            /// <summary>
            /// 备注信息
            /// </summary>
            public string Description;

            /// <summary>
            /// 自定义类型
            /// </summary>
            public string Type;

            /// <summary>
            /// 是否自增
            /// </summary>
            public bool Identity;

            /// <summary>
            /// 是否主键
            /// </summary>
            public bool Key;

            public override string ToString()
            {
                List<string> att = new List<string>();
                att.Add(string.Format("Column(\"{0}\")", this.ColumnName));
                if (this.Identity) att.Add("DatabaseGenerated(DatabaseGeneratedOption.Identity)");
                if (this.Key) att.Add("Key");

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Description))
                {
                    sb.AppendLine($@"        /// <summary>
        /// {this.Description}
        /// </summary>");
                }
                sb.AppendLine($"        [{ string.Join(",", att) }]")
                    .AppendLine($"        public {this.Type} {this.Name} {{ get; set; }}");

                return sb.ToString();
            }
        }


    }
}
