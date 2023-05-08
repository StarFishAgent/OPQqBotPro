using System;
using System.Collections.Generic;

namespace GenshinBotCore.Entities
{
    public partial class Pictures
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string Picture { get; set; } = null!;
    }
}
