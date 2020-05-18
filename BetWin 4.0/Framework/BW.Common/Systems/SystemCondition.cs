using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.StudioCore.Model;

namespace BW.Common.Systems
{
    /// <summary>
    /// 数据库条件筛选配置
    /// </summary>
    [Table("sys_Condition")]
    public partial class SystemCondition
    {

        #region  ========  構造函數  ========
        public SystemCondition() { }

        public SystemCondition(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ConditionID":
                        this.ID = (int)reader[i];
                        break;
                    case "ConditionName":
                        this.Name = (string)reader[i];
                        break;
                    case "ConditionDescription":
                        this.Description = (string)reader[i];
                        break;
                    case "Type":
                        this.Type = (ConditionType)reader[i];
                        break;
                    case "Condition":
                        this.Condition = (string)reader[i];
                        break;
                    case "ParamSetting":
                        this.ParamString = (string)reader[i];
                        break;
                    case "Sort":
                        this.Sort = (short)reader[i];
                        break;
                }
            }
        }


        public SystemCondition(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ConditionID":
                        this.ID = (int)dr[i];
                        break;
                    case "ConditionName":
                        this.Name = (string)dr[i];
                        break;
                    case "ConditionDescription":
                        this.Description = (string)dr[i];
                        break;
                    case "Type":
                        this.Type = (ConditionType)dr[i];
                        break;
                    case "Condition":
                        this.Condition = (string)dr[i];
                        break;
                    case "ParamSetting":
                        this.ParamString = (string)dr[i];
                        break;
                    case "Sort":
                        this.Sort = (short)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 条件配置编号
        /// </summary>
        [Column("ConditionID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 条件名称
        /// </summary>
        [Column("ConditionName")]
        public string Name { get; set; }


        /// <summary>
        /// 条件描述
        /// </summary>
        [Column("ConditionDescription")]
        public string Description { get; set; }


        /// <summary>
        /// 条件配置类型
        /// </summary>
        [Column("Type")]
        public ConditionType Type { get; set; }


        /// <summary>
        /// 条件配置内容（SQL语句）
        /// </summary>
        [Column("Condition")]
        public string Condition { get; set; }


        /// <summary>
        /// 参数设置
        /// </summary>
        [Column("ParamSetting")]
        public string ParamString { get; set; }


        /// <summary>
        /// 排序值（从大到小)
        /// </summary>
        [Column("Sort")]
        public short Sort { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        private ParamSetting _setting;

        [NotMapped]
        public ParamSetting Setting
        {
            get
            {
                if (_setting == null)
                {
                    _setting = new ParamSetting(this.ParamString);
                }
                return _setting;
            }
            set
            {
                this.ParamString = _setting = value;
            }
        }

        public class ParamSetting : IJsonSetting
        {
            public ParamSetting()
            {
            }

            public ParamSetting(string jsonString) : base(jsonString)
            {
            }

            /// <summary>
            /// 参数名
            /// </summary>
            public string ParamName { get; set; }

            /// <summary>
            /// 名称描述
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 参数类型
            /// </summary>
            public ParamType Type { get; set; }

            /// <summary>
            /// 设定值
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// 如果Type = Select，此处配置可选项
            /// </summary>
            public string[] Options { get; set; }
        }

        /// <summary>
        /// 条件类型
        /// </summary>
        public enum ConditionType : byte
        {
            /// <summary>
            /// 会员分组
            /// </summary>
            [Description("会员分组")]
            UserGroup,
            /// <summary>
            /// VIP等级
            /// </summary>
            [Description("VIP等级")]
            VIPLevel
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        public enum ParamType
        {
            /// <summary>
            /// 数字
            /// </summary>
            [Description("数字")]
            Number,
            /// <summary>
            /// 选择项
            /// </summary>
            [Description("选择项")]
            Select
        }
        #endregion

    }

}
