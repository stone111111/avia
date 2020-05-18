using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent.Translates
{
    /// <summary>
    /// 频道
    /// </summary>
    [Table("tran_Channel")]
    public class TranslateChannel
    {
        [Column("ChannelID"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("ChannelName")]
        public string Name { get; set; }

        [Column("Status")]
        public ChannelStatus Status { get; set; }
    }

    public enum ChannelStatus : byte
    {
        [Description("正常")]
        Normal = 0,
        [Description("待发布")]
        Publish = 1
    }
}
