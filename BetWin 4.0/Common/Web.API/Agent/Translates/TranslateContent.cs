using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent.Translates
{
    /// <summary>
    /// 翻译内容
    /// </summary>
    [Table("tran_Content")]
    public class TranslateContent
    {
        [Column("KeyID"), Key]
        public string KeyID { get; set; }

        [Column("Language"), Key]
        public Language Language { get; set; }

        [Column("Content")]
        public string Content { get; set; }
    }
}
