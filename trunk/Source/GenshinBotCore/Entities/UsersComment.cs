using System;
using System.Collections.Generic;

namespace GenshinBotCore.Entities
{
    public partial class UsersComment
    {
        public int Id { get; set; }
        public int QQ { get; set; }
        public string VideoUrl { get; set; } = null!;
        public string Time { get; set; } = null!;
    }
}
