using System;
using System.Collections.Generic;

namespace GenshinBotCore.Entities
{
    public partial class UsersCookies
    {
        public int Id { get; set; }
        public string? Cookies { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; } = null!;
    }
}
