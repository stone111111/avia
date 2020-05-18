using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using BW.Views;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Utils;
using SP.StudioCore.Web;
using SP.StudioCore.Security;

namespace BW.Common.Views
{
    /// <summary>
    /// 视图的模型
    /// </summary>
    [Table("view_Model")]
    public partial class ViewModel
    {

        #region  ========  構造函數  ========
        public ViewModel() { }

        public ViewModel(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ModelID":
                        this.ID = (int)reader[i];
                        break;
                    case "ViewID":
                        this.ViewID = (int)reader[i];
                        break;
                    case "ModelName":
                        this.Name = (string)reader[i];
                        break;
                    case "ModelDesc":
                        this.Description = (string)reader[i];
                        break;
                    case "Preview":
                        this.Preview = (string)reader[i];
                        break;
                    case "Path":
                        this.Path = (string)reader[i];
                        break;
                    case "Page":
                        this.Page = (string)reader[i];
                        break;
                    case "Style":
                        this.Style = (string)reader[i];
                        break;
                    case "Resources":
                        this.Resources = (string)reader[i];
                        break;
                }
            }
        }


        public ViewModel(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ModelID":
                        this.ID = (int)dr[i];
                        break;
                    case "ViewID":
                        this.ViewID = (int)dr[i];
                        break;
                    case "ModelName":
                        this.Name = (string)dr[i];
                        break;
                    case "ModelDesc":
                        this.Description = (string)dr[i];
                        break;
                    case "Preview":
                        this.Preview = (string)dr[i];
                        break;
                    case "Path":
                        this.Path = (string)dr[i];
                        break;
                    case "Page":
                        this.Page = (string)dr[i];
                        break;
                    case "Style":
                        this.Style = (string)dr[i];
                        break;
                    case "Resources":
                        this.Resources = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 视图模板编号
        /// </summary>
        [Column("ModelID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所属的视图
        /// </summary>
        [Column("ViewID")]
        public int ViewID { get; set; }


        /// <summary>
        /// 视图模型名称
        /// </summary>
        [Column("ModelName")]
        public string Name { get; set; }


        /// <summary>
        /// 视图模板的配置说明（来自于页面文件内，格式：<!-- 说明内容 -->）
        /// </summary>
        [Column("ModelDesc")]
        public string Description { get; set; }


        /// <summary>
        /// 预览图
        /// </summary>
        [Column("Preview")]
        public string Preview { get; set; }


        /// <summary>
        /// 视图模型的路径，在PC和H5环境中用于测试，在APP中用于定位视图资源的路径/名称
        /// </summary>
        [Column("Path")]
        public string Path { get; set; }


        /// <summary>
        /// 页面文件内容
        /// </summary>
        [Column("Page")]
        public string Page { get; set; }


        /// <summary>
        /// 样式文件内容
        /// </summary>
        [Column("Style")]
        public string Style { get; set; }


        /// <summary>
        /// 资源文件路径，格式：{"logo.png":"images/{MD5}.png"}
        /// </summary>
        [Column("Resources")]
        public string Resources { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        private Dictionary<string, ResourceFile> _resourceFiles;
        /// <summary>
        /// 资源路径
        /// </summary>
        [NotMapped]
        public Dictionary<string, ResourceFile> ResourceFiles
        {
            get
            {
                if (_resourceFiles == null)
                {
                    try
                    {
                        _resourceFiles = JsonConvert.DeserializeObject<Dictionary<string, ResourceFile>>(string.IsNullOrEmpty(this.Resources) ? "{}" : this.Resources);
                    }
                    catch
                    {
                        _resourceFiles = new Dictionary<string, ResourceFile>();
                    }
                }
                return this._resourceFiles;
            }
            set
            {
                _resourceFiles = value;
                this.Resources = JsonConvert.SerializeObject(this._resourceFiles);
            }
        }

        /// <summary>
        /// 资源文件的格式
        /// </summary>
        public struct ResourceFile
        {
            public ResourceFile(IFormFile file, byte[] data)
            {
                string md5 = Encryption.toMD5(data);
                this.Name = file.FileName;
                this.Size = (int)file.Length;
                this.Path = string.Concat("resources/", Encryption.toMD5Short(md5), this.Name.Substring(this.Name.LastIndexOf('.')));
            }

            /// <summary>
            /// 文件名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 文件大小
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// 路径
            /// </summary>
            public string Path { get; set; }
        }

        #endregion

    }

}
