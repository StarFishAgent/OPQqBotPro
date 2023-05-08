using GenshinBotCore.Entities;
using GenshinBotCore.Services;
using System;
using System.Threading.Tasks;

namespace GenshinBotCoreTests.Mocks
{
    public class TestUserManager : IUserManager
    {
        private static readonly Users user = new Users
        {
            GenshinUid = "",
            Id = 0,
            MihoyoId = "",
            QQ = 0
        };
        private static readonly UsersComment usersComment = new UsersComment
        {

        };
        private static readonly UsersCookies usersCookies = new UsersCookies
        {

        };
        public Users? GetUserByGenshinUid(string genshinUid)
        {
            return user;
        }

        public Users? GetUserById(int id)
        {
            return user;
        }

        public Users? GetUserByMihoyoId(string mihoyoId)
        {
            return user;
        }

        public Users? GetUserByQQ(long qqId)
        {
            return user;
        }
        public Users? GetUserInfo()
        {
            return user;
        }
        public UsersComment? GetUserByQQComment(long qq)
        {
            return usersComment;
        }
        public UsersCookies? GetUserByQQCookies(int cookies)
        {
            return usersCookies;
        }

        public Task<Users?> UpdateUserAsync(Users user)
        {
            throw new NotImplementedException();
        }
        public Task<UsersComment?> UpdateUserCommentAsync(UsersComment user)
        {
            throw new NotImplementedException();
        }
        public Task<UsersCookies?> UpdateUserCookiesAsync(UsersCookies user)
        {
            throw new NotImplementedException();
        }
    }
}
