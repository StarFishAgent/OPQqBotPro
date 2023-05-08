using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;



namespace YukinoshitaBot.Entities
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=GenshinBot.db");
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// 用户视频信息表
        /// </summary>
        public DbSet<UsersComment> UsersComment { get; set; } = null!;

        /// <summary>
        /// 用户Cookies信息表
        /// </summary>
        public DbSet<UsersCookies> UsersCookies { get; set; } = null!;

        /// <summary>
        /// 用户密钥表
        /// </summary>
        public DbSet<UserSecret> UsersSecret { get; set; } = null!;

        /// <summary>
        /// 图片缓存
        /// </summary>
        public DbSet<Pictures> Pictures { get; set; } = null!;
    }
}
