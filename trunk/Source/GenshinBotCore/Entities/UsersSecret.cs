using System;
using System.Collections.Generic;

namespace GenshinBotCore.Entities
{
    public partial class UsersSecret
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int UserId { get; set; }

        public virtual Users User { get; set; } = null!;
    }
}
