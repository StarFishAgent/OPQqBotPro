using GenshinBotCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenshinBotCore.Services
{
    public class UserManager : IUserManager
    {
        public UserManager(GenShinBotContext dbContext, ILogger<UserManager> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        private readonly GenShinBotContext dbContext;
        private readonly ILogger logger;

        public Users? GetUserByGenshinUid(string genshinUid) =>
            dbContext.Users.Where(u => u.GenshinUid == genshinUid).AsNoTracking().SingleOrDefault();

        public Users? GetUserById(int id) =>
            dbContext.Users.Where(u => u.Id == id).AsNoTracking().SingleOrDefault();

        public Users? GetUserByMihoyoId(string mihoyoId) =>
            dbContext.Users.Where(u => u.MihoyoId == mihoyoId).AsNoTracking().SingleOrDefault();

        public Users? GetUserByQQ(long qqId) =>
            dbContext.Users.Where(u => u.QQ == qqId).AsNoTracking().SingleOrDefault();
        public Users? GetUserInfo() =>
           dbContext.Users.AsNoTracking().SingleOrDefault();
        public UsersComment? GetUserByQQComment(long qqId) =>
            dbContext.UsersComment.Where(u => u.QQ == qqId).AsNoTracking().SingleOrDefault();
        public UsersCookies? GetUserByQQCookies(int UserId) =>
            dbContext.UsersCookies.Where(u => u.UserId == UserId).AsNoTracking().SingleOrDefault();
        /// <summary>
        /// 修改users表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users?> UpdateUserAsync(Users user)
        {
            try
            {
                var exsistUser = GetUserByQQ(user.QQ);
                if (exsistUser is null)
                {
                    exsistUser = dbContext.Users.Add(user).Entity;
                }
                exsistUser.GenshinUid = user.GenshinUid ?? "";
                exsistUser.MihoyoId = user.MihoyoId ?? "";
                exsistUser.ServerId = user.ServerId ?? "";

                user = exsistUser;
            }
            catch (Exception e)
            {
                logger.LogDebug(e.Message);
                return null;
            }
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }

        /// <summary>
        /// 修改usersComment表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UsersComment?> UpdateUserCommentAsync(UsersComment user)
        {
            try
            {
                var exsistUser = GetUserByQQComment(user.QQ);
                if (exsistUser is null)
                {
                    exsistUser = dbContext.UsersComment.Add(user).Entity;
                }
                else
                {
                    exsistUser.QQ = user.QQ;
                    exsistUser.VideoUrl = user.VideoUrl;
                    exsistUser.Time = user.Time;
                    user = exsistUser;
                    dbContext.Entry(user).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {
                logger.LogDebug(e.Message);
                return null;
            }
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }

        /// <summary>
        /// 修改usersCookies表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UsersCookies?> UpdateUserCookiesAsync(UsersCookies user)
        {
            try
            {
                var exsistUser = GetUserByQQCookies(user.UserId);
                if (exsistUser is null)
                {
                    exsistUser = dbContext.UsersCookies.Add(user).Entity;
                }
                else
                {
                    exsistUser.Cookies = user.Cookies;
                    user = exsistUser;
                    dbContext.Entry(user).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {
                logger.LogDebug(e.Message);
                return null;
            }
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }
    }
}
