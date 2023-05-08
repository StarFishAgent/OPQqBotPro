using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YukinoshitaBot.Entities;

namespace YukinoshitaBot.Services.UserManager
{
    public class UserManager : IUserManager
    {
        public UserManager(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public User? GetUserByGenshinUid(string genshinUid) =>
            dbContext.Users.Where(u => u.GenshinUid == genshinUid).AsNoTracking().SingleOrDefault();

        public User? GetUserById(int id) =>
            dbContext.Users.Where(u => u.Id == id).AsNoTracking().SingleOrDefault();

        public User? GetUserByMihoyoId(string mihoyoId) =>
            dbContext.Users.Where(u => u.MihoyoId == mihoyoId).AsNoTracking().SingleOrDefault();

        public User? GetUserByQQ(long qqId) =>
            dbContext.Users.Where(u => u.QQ == qqId).AsNoTracking().SingleOrDefault();
        public UsersComment? GetUserByQQComment(long qqId) =>
            dbContext.UsersComment.Where(u => u.QQ == qqId).AsNoTracking().SingleOrDefault();

        /// <summary>
        /// 修改users表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User?> UpdateUserAsync(User user)
        {
            try
            {
                var exsistUser = GetUserByQQ(user.QQ);
                if (exsistUser is null)
                {
                    exsistUser = dbContext.Users.Add(user).Entity;
                }
                exsistUser.GenshinUid = user.GenshinUid;
                exsistUser.MihoyoId = user.MihoyoId;

                user = exsistUser;
            }
            catch (Exception e)
            {
                //logger.LogDebug(e.Message);
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
                //logger.LogDebug(e.Message);
                return null;
            }
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }
    }
}
