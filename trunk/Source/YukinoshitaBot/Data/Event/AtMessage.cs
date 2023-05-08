using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using YukinoshitaBot.Entities;
using YukinoshitaBot.Services.UserManager;

namespace YukinoshitaBot.Data.Event
{
    public class AtMessage : Message
    {
        public AtMessage(SenderInfo sender, string content, long FromUin ) : base(sender)
        {
            string[] urls = content.Split(new string[] { "feed_key=" }, StringSplitOptions.RemoveEmptyEntries);
            string[] url = urls[1].Split('\\');
            var VideoURL = "https://xsj.qq.com/video?feed_key=" + url[0];
            InsertSqliteDB(VideoURL, FromUin);
        }
        public async void InsertSqliteDB(string content, long QQ)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            UserManager userManager = new UserManager(dbContext);
           await userManager.UpdateUserCommentAsync(new Entities.UsersComment
            {
                QQ = QQ,
                VideoUrl = content,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH-mm")
            }); ;
            
        }


    }
}
