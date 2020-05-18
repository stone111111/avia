using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent.Translates
{
    [Table("tran_Token")]
    public class TranslateToken
    {
        [Column("ChannelID"), Key]
        public int ChannelID { get; set; }

        [Column("KeyID"), Key]
        public string KeyID { get; set; }
    }
}
