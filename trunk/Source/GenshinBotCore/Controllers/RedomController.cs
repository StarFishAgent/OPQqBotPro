using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using YukinoshitaBot.Data.Attributes;
using GenshinBotCore.Services;
using YukinoshitaBot.Data.Controller;
using System.Text.RegularExpressions;
using YukinoshitaBot.Data.Event;

namespace GenshinBotCore.Controllers
{
    [StrictRoute(Command = "骰子", Priority = 6)]
    public class RedomController : BotControllerBase
    {
        public RedomController(
           IUserManager userManager)
        {
            this.userManager = userManager;
        }
        private readonly IUserManager userManager;
        [FriendText, GroupText]
        public void Sent()
        {
            var message = Message as TextMessage ?? throw new NullReferenceException();
            var user = message.SenderInfo.FromNickName;
            Random random = new Random();
            var num = random.Next(1, 6);
            ReplyTextMsg(user + "你的点数是" + num);
        }
    }
}
