using System;
using System.Collections.Generic;

namespace GenshinBotCore.Entities
{
    public partial class Users
    {
        public Users()
        {
            UsersCookies = new HashSet<UsersCookies>();
            UsersSecrets = new HashSet<UsersSecret>();
        }

        public int Id { get; set; }
        public long QQ { get; set; }
        public string MihoyoId { get; set; } = null!;
        public string GenshinUid { get; set; } = null!;
        public string ServerId { get; set; } = null!;

        public virtual ICollection<UsersCookies> UsersCookies { get; set; }
        public virtual ICollection<UsersSecret> UsersSecrets { get; set; }
    }
}
