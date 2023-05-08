using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoshitaBot.Entities
{
    public class UsersComment
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        [Required]
        public long QQ { get; set; }

        /// <summary>
        /// 视频URL
        /// </summary>
        [Required]
        public string VideoUrl { get; set; } = string.Empty;

        /// <summary>
        /// 时间
        /// </summary>
        [Required]
        public string Time { get; set; } = string.Empty;
    }
}
