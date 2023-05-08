using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using GerminationTranslate;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;

namespace GenshinBotCore.Controllers
{
    [StartRoute(Command = "翻译", Priority = 7)]
    public class TransController : BotControllerBase
    {
        [FriendText, GroupText]
        public void Trans()
        {
            Trans trans = new Trans();
            var cmd = (this.Message as TextMessage)?.Content.Split(' ') ?? throw new NullReferenceException();
            var Text = "";
            try
            {
                if (trans.IsTrans(cmd[cmd.Length - 1]))
                {
                    for (int i = 1; i < cmd.Length - 1; i++)
                    {
                        Text += " " + cmd[i];
                    }
                    ReplyTextMsg(trans.TransText(Text, "auto", cmd[cmd.Length - 1]).Trim());
                }
                else
                {
                    for (int i = 1; i < cmd.Length; i++)
                    {
                        Text += " " + cmd[i];
                    }
                    ReplyTextMsg(trans.TransText(Text, "auto", "zh").Trim());
                }
            }
            catch 
            {
                ReplyTextMsg("翻译出错啦");
            }
            
        }
    }
}
