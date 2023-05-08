using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinBotCore.Extensions;
using GenshinBotCore.Info;
using GenshinBotCore.Services;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;
using YukinoshitaBot.Data.OpqApi;


namespace GenshinBotCore.Controllers
{
    [StartRoute(Command = "刷评论", Priority = 8)]
    internal class AddCommentController : BotControllerBase
    {
        public AddCommentController(
           IUserManager userManager
           )
        {
            this.userManager = userManager;
        }

        private readonly IUserManager userManager;

        [FriendText]
        public async Task AddComments()
        {
            var QQnum = Message.SenderInfo.FromQQ.ToString() ?? "";
            if (string.IsNullOrEmpty(QQnum))
            {
                return;
            }
            if (StaticInfo.lisList.Count < 5)
            {
                if (StaticInfo.lisList.Count == 0)
                {
                    StaticInfo.lisList.Add(QQnum);
                    var VideoUrl = userManager.GetUserByQQComment(Convert.ToInt64(QQnum)).VideoUrl;
                    await IniAddComment(VideoUrl, QQnum);
                    StaticInfo.lisList.Remove(QQnum);
                    return;
                }
                if (IsExists(QQnum) == true)
                {
                    StaticInfo.lisList.Add(QQnum);
                    var VideoUrl = userManager.GetUserByQQComment(Convert.ToInt64(QQnum)).VideoUrl;
                    await IniAddComment(VideoUrl, QQnum);
                    StaticInfo.lisList.Remove(QQnum);
                    return;
                }
                else
                {
                    ReplyTextMsg("您已经在在使用了");
                }
            }
            else
            {
                ReplyTextMsg("人数已满请稍后再试");
            }


        }

        public bool IsExists(string QQnum)
        {
            foreach (var item in StaticInfo.lisList)
            {
                if (item != QQnum)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task IniAddComment(string VideoUrl, string QQ)
        {
            AddCommentExtensions.CreateDire();
            await AddCommentExtensions.CheckDownloadBrowser();
            await AddCommentExtensions.CreateBrowser();
            var Result = await AddCommentExtensions.IniPage(VideoUrl, QQ);
            var CookiesCount = await AddCommentExtensions.GetPageCookiesCount(Result);
            var image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "QRPic\\" + QQ + ".png");
            var pic = ConvertImageToBase64(image);
            this.Message.Reply(new PictureMessageRequest(pic));
            ReplyTextMsg("请扫码登录");
            ReplyTextMsg(await AddCommentExtensions.ForeachIsLogin(Result, CookiesCount));
            await AddCommentExtensions.LoadAddComment(Result, CookiesCount);
            ReplyTextMsg("实例运行完毕，欢迎下次使用");
        }

        public string ConvertImageToBase64(Image file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.Save(memoryStream, file.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
