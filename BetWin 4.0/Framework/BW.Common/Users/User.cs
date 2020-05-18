using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using System.ComponentModel;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using System.Linq;

namespace BW.Common.Users
{
    /// <summary>
    /// 会员
    /// </summary>
    [Table("Users")]
    public partial class User
    {

        #region  ========  構造函數  ========
        public User() { }

        public User(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "UserID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "UserName":
                        this.UserName = (string)reader[i];
                        break;
                    case "Password":
                        this.Password = (string)reader[i];
                        break;
                    case "PayPassword":
                        this.PayPassword = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "RegIP":
                        this.RegIP = (string)reader[i];
                        break;
                    case "RegDomain":
                        this.RegDomain = (string)reader[i];
                        break;
                    case "RegPlatform":
                        this.RegPlatform = (PlatformType)reader[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)reader[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)reader[i];
                        break;
                    case "LoginDomain":
                        this.LoginDomain = (string)reader[i];
                        break;
                    case "LoginPlatform":
                        this.LoginPlatform = (PlatformType)reader[i];
                        break;
                    case "Currency":
                        this.Currency = (Currency)reader[i];
                        break;
                    case "Money":
                        this.Money = (decimal)reader[i];
                        break;
                    case "LockMoney":
                        this.LockMoney = (decimal)reader[i];
                        break;
                    case "GameMoney":
                        this.GameMoney = (decimal)reader[i];
                        break;
                    case "Language":
                        this.Language = (Language)reader[i];
                        break;
                    case "AgentID":
                        this.AgentID = (int)reader[i];
                        break;
                    case "InviteID":
                        this.InviteID = (int)reader[i];
                        break;
                    case "Lock":
                        this.Lock = (LockType)reader[i];
                        break;
                    case "Function":
                        this.Function = (FunctionType)reader[i];
                        break;
                    case "Type":
                        this.Type = (UserType)reader[i];
                        break;
                    case "IsTest":
                        this.IsTest = (bool)reader[i];
                        break;
                    case "Mobile":
                        this.Mobile = (string)reader[i];
                        break;
                    case "IsMobile":
                        this.IsMobile = (bool)reader[i];
                        break;
                    case "Email":
                        this.Email = (string)reader[i];
                        break;
                    case "IsEmail":
                        this.IsEmail = (bool)reader[i];
                        break;
                    case "Face":
                        this.Face = (string)reader[i];
                        break;
                    case "NickName":
                        this.NickName = (string)reader[i];
                        break;
                    case "GroupID":
                        this.GroupID = (int)reader[i];
                        break;
                    case "Question":
                        this.Question = (QuestionType)reader[i];
                        break;
                    case "Answer":
                        this.Answer = (string)reader[i];
                        break;
                    case "LevelID":
                        this.LevelID = (int)reader[i];
                        break;
                    case "AccountName":
                        this.AccountName = (string)reader[i];
                        break;
                    case "Time":
                        this.Time = (byte[])reader[i];
                        break;
                }
            }
        }


        public User(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "UserID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "UserName":
                        this.UserName = (string)dr[i];
                        break;
                    case "Password":
                        this.Password = (string)dr[i];
                        break;
                    case "PayPassword":
                        this.PayPassword = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "RegIP":
                        this.RegIP = (string)dr[i];
                        break;
                    case "RegDomain":
                        this.RegDomain = (string)dr[i];
                        break;
                    case "RegPlatform":
                        this.RegPlatform = (PlatformType)dr[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)dr[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)dr[i];
                        break;
                    case "LoginDomain":
                        this.LoginDomain = (string)dr[i];
                        break;
                    case "LoginPlatform":
                        this.LoginPlatform = (PlatformType)dr[i];
                        break;
                    case "Currency":
                        this.Currency = (Currency)dr[i];
                        break;
                    case "Money":
                        this.Money = (decimal)dr[i];
                        break;
                    case "LockMoney":
                        this.LockMoney = (decimal)dr[i];
                        break;
                    case "GameMoney":
                        this.GameMoney = (decimal)dr[i];
                        break;
                    case "Language":
                        this.Language = (Language)dr[i];
                        break;
                    case "AgentID":
                        this.AgentID = (int)dr[i];
                        break;
                    case "InviteID":
                        this.InviteID = (int)dr[i];
                        break;
                    case "Lock":
                        this.Lock = (LockType)dr[i];
                        break;
                    case "Function":
                        this.Function = (FunctionType)dr[i];
                        break;
                    case "Type":
                        this.Type = (UserType)dr[i];
                        break;
                    case "IsTest":
                        this.IsTest = (bool)dr[i];
                        break;
                    case "Mobile":
                        this.Mobile = (string)dr[i];
                        break;
                    case "IsMobile":
                        this.IsMobile = (bool)dr[i];
                        break;
                    case "Email":
                        this.Email = (string)dr[i];
                        break;
                    case "IsEmail":
                        this.IsEmail = (bool)dr[i];
                        break;
                    case "Face":
                        this.Face = (string)dr[i];
                        break;
                    case "NickName":
                        this.NickName = (string)dr[i];
                        break;
                    case "GroupID":
                        this.GroupID = (int)dr[i];
                        break;
                    case "Question":
                        this.Question = (QuestionType)dr[i];
                        break;
                    case "Answer":
                        this.Answer = (string)dr[i];
                        break;
                    case "LevelID":
                        this.LevelID = (int)dr[i];
                        break;
                    case "AccountName":
                        this.AccountName = (string)dr[i];
                        break;
                    case "Time":
                        this.Time = (byte[])dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("UserID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 用户名（只允许数字+字母）
        /// </summary>
        [Column("UserName")]
        public string UserName { get; set; }


        /// <summary>
        /// 密码（MD5加密）
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }


        /// <summary>
        /// 资金密码（SHA+MD5双重加密），6位数字格式
        /// </summary>
        [Column("PayPassword")]
        public string PayPassword { get; set; }


        /// <summary>
        /// 注册时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 注册IP
        /// </summary>
        [Column("RegIP")]
        public string RegIP { get; set; }


        /// <summary>
        /// 注册来源域名
        /// </summary>
        [Column("RegDomain")]
        public string RegDomain { get; set; }


        /// <summary>
        /// 注册设备
        /// </summary>
        [Column("RegPlatform")]
        public PlatformType RegPlatform { get; set; }


        /// <summary>
        /// 最近一次登录的时间
        /// </summary>
        [Column("LoginAt")]
        public DateTime LoginAt { get; set; }


        /// <summary>
        /// 登录IP
        /// </summary>
        [Column("LoginIP")]
        public string LoginIP { get; set; }


        /// <summary>
        /// 登录域名
        /// </summary>
        [Column("LoginDomain")]
        public string LoginDomain { get; set; }


        /// <summary>
        /// 登录设备
        /// </summary>
        [Column("LoginPlatform")]
        public PlatformType LoginPlatform { get; set; }


        /// <summary>
        /// 会员币种（默认继承自上级代理的币种）
        /// </summary>
        [Column("Currency")]
        public Currency Currency { get; set; }


        /// <summary>
        /// 当前可用余额（非时时）
        /// </summary>
        [Column("Money")]
        public decimal Money { get; set; }


        /// <summary>
        /// 当前锁定金额（非实时）
        /// </summary>
        [Column("LockMoney")]
        public decimal LockMoney { get; set; }


        /// <summary>
        /// 当前游戏账户总余额（非实时)
        /// </summary>
        [Column("GameMoney")]
        public decimal GameMoney { get; set; }


        /// <summary>
        /// 默认语种（继承自上级代理的语种）
        /// </summary>
        [Column("Language")]
        public Language Language { get; set; }


        /// <summary>
        /// 上级代理ID
        /// </summary>
        [Column("AgentID")]
        public int AgentID { get; set; }


        /// <summary>
        /// 邀请者ID
        /// </summary>
        [Column("InviteID")]
        public int InviteID { get; set; }


        /// <summary>
        /// 当前会员被锁定的功能（位枚举）
        /// </summary>
        [Column("Lock")]
        public LockType Lock { get; set; }


        /// <summary>
        /// 当前会员可开放的功能
        /// </summary>
        [Column("Function")]
        public FunctionType Function { get; set; }


        /// <summary>
        /// 会员类型，股东、总代、代理、会员
        /// </summary>
        [Column("Type")]
        public UserType Type { get; set; }


        /// <summary>
        /// 标记为测试账号（测试账号必须整条线都是测试账号，不可更改)
        /// </summary>
        [Column("IsTest")]
        public bool IsTest { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("Mobile")]
        public string Mobile { get; set; }


        /// <summary>
        /// 手机是否通过了验证
        /// </summary>
        [Column("IsMobile")]
        public bool IsMobile { get; set; }


        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }


        /// <summary>
        /// 邮箱是否通过验证
        /// </summary>
        [Column("IsEmail")]
        public bool IsEmail { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [Column("Face")]
        public string Face { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        [Column("NickName")]
        public string NickName { get; set; }


        /// <summary>
        /// 所属分组
        /// </summary>
        [Column("GroupID")]
        public int GroupID { get; set; }


        /// <summary>
        /// 安全问题
        /// </summary>
        [Column("Question")]
        public QuestionType Question { get; set; }


        /// <summary>
        /// 安全问题答案
        /// </summary>
        [Column("Answer")]
        public string Answer { get; set; }


        /// <summary>
        /// 所处的VIP等级（可以为0），对应 usr_Level 表的ID
        /// </summary>
        [Column("LevelID")]
        public int LevelID { get; set; }


        /// <summary>
        /// 提现的银行卡姓名
        /// </summary>
        [Column("AccountName")]
        public string AccountName { get; set; }


        /// <summary>
        /// 更改的时间戳，唯一标记
        /// </summary>
        [Column("Time")]
        public byte[] Time { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 是否设置了资金密码
        /// </summary>
        [NotMapped]
        public bool IsPayPassword
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.PayPassword);
            }
        }

        /// <summary>
        /// 是否设置了安全问题
        /// </summary>
        [NotMapped]
        public bool IsQuestion
        {
            get
            {
                return this.Question != QuestionType.None;
            }
        }

        /// <summary>
        /// 对用户锁定的功能
        /// </summary>
        [Flags]
        public enum LockType : byte
        {
            None = 0,
            /// <summary>
            /// 禁止登录
            /// </summary>
            [Description("登录")]
            Login = 1,
            /// <summary>
            /// 禁止发起充值
            /// </summary>
            [Description("充值")]
            Recharge = 2,
            /// <summary>
            /// 禁止发起提现
            /// </summary>
            [Description("提现")]
            Withdraw = 4,
            /// <summary>
            /// 锁定该项之后用户得不到返水
            /// </summary>
            [Description("返水")]
            Return = 8,
            /// <summary>
            /// 锁定该项之后用户得不到返佣
            /// </summary>
            [Description("返佣")]
            Commission = 16,
            /// <summary>
            /// 锁定该项之后用户不能进行游戏，也不能进行游戏资金的转入转出操作
            /// </summary>
            [Description("游戏")]
            Game = 32,
            /// <summary>
            /// 没有执行契约，此项被锁定，不能提现和游戏
            /// </summary>
            [Description("契约")]
            Contract = 64
        }

        /// <summary>
        /// 单独开放的功能
        /// </summary>
        [Flags]
        public enum FunctionType : byte
        {
            None = 0,
            /// <summary>
            /// 允许对下级进行转账操作
            /// </summary>
            [Description("下级转账")]
            SubTransfer = 1,
            /// <summary>
            /// 允许自由转账给任何人
            /// </summary>
            [Description("自由转账")]
            FreeTransfer = 2,
            /// <summary>
            /// 有升级直属会员成为代理的权限
            /// </summary>
            [Description("审核代理")]
            CheckAgent = 4
        }

        /// <summary>
        /// 会员类型
        /// </summary>
        public enum UserType : byte
        {
            /// <summary>
            /// 直属于平台的代理账号，一般由内部人员担任
            /// </summary>
            [Description("股东")]
            Partner = 0,
            /// <summary>
            /// 直属于股东的代理账号，其下级只能是代理。
            /// </summary>
            [Description("总代")]
            Agent = 1,
            /// <summary>
            /// 总代之后的代理，可发展无限层级。
            /// </summary>
            [Description("代理")]
            Broker = 2,
            /// <summary>
            /// 代理的下属账号，没有发展下级的权限。可升级成为代理（不可逆）。
            /// </summary>
            [Description("会员")]
            Member = 3
        }

        /// <summary>
        /// 密码问题
        /// </summary>
        public enum QuestionType : byte
        {
            [Description("未设置")]
            None,
            [Description("您的母亲姓名是？")]
            Q1,
            [Description("您的父亲姓名是？")]
            Q2,
            [Description("您的配偶姓名是？")]
            Q3,
            [Description("您的配偶生日是？")]
            Q4,
            [Description("您的学号（或工号）是？")]
            Q5,
            [Description("您的小学班主任姓名是？")]
            Q6,
            [Description("您的初中班主任姓名是？")]
            Q7,
            [Description("您的高中班主任姓名是？")]
            Q8,
            [Description("您最熟悉的童年好友姓名是？")]
            Q9
        }

        public static implicit operator User(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            return hashes.Fill<User>();
        }

        public static implicit operator HashEntry[](User user)
        {
            if (user == null || user.ID == 0) return null;
            return user.ToHashEntry().ToArray();
        }

        #endregion

    }

}
