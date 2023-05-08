using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukinoshitaBot.Entities
{
    /// <summary>
    /// 用户cookies模型
    /// </summary>
    public class UsersCookies
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 密钥名称
        /// </summary>
        [Required]
        public string Cookies { get; set; } = string.Empty;

        /// <summary>
        /// 从属用户
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary>
        /// 从属用户的Id
        /// </summary>
        public int UserId { get; set; }
    }
}
