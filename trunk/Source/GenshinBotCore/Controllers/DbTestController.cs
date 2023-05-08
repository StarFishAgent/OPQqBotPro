using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenshinBotCore.Services;

using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GenshinBotCore.Controllers
{
    [StrictRoute(Command = "测试", Priority = 100)]
    internal class DbTestController : BotControllerBase
    {
        public DbTestController(
            ILogger<DbTestController> logger,
            ITakumiApi takumiApi,
            IMihoyoApi mihoyoApi,
            IUserManager userManager,
            ISecretManager secretManager)
        {
            this.logger = logger;
            this.takumiApi = takumiApi;
            this.mihoyoApi = mihoyoApi;
            this.userManager = userManager;
            this.secretManager = secretManager;
        }
        private readonly ILogger logger;
        private readonly ITakumiApi takumiApi;
        private readonly IMihoyoApi mihoyoApi;
        private readonly IUserManager userManager;
        private readonly ISecretManager secretManager;

        [FriendText]
        public async Task AddInfo()
        {
            var message = Message as TextMessage ?? throw new NullReferenceException();
            var user = await userManager.UpdateUserAsync(new Entities.Users
            {
                QQ = Convert.ToInt32(message.SenderInfo.FromQQ ?? default),
                MihoyoId = "MihoyoId",
            });
            user.GenshinUid = "GenshinUid";
            user.ServerId = "ServerId";
            user = await userManager.UpdateUserAsync(user);
        }
    }
}
