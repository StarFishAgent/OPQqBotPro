using GenshinBotCore.Entities;

namespace GenshinBotCore.Services
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    public interface IUserManager
    {
        Users? GetUserById(int id);
        Users? GetUserByQQ(long qqId);
        Users? GetUserByMihoyoId(string mihoyoId);
        Users? GetUserByGenshinUid(string genshinUid);
        Users? GetUserInfo();
        UsersComment? GetUserByQQComment(long QQ);
        UsersCookies? GetUserByQQCookies(int UserId);
        Task<Users?> UpdateUserAsync(Users user);
        Task<UsersComment?> UpdateUserCommentAsync(UsersComment user);
        Task<UsersCookies?> UpdateUserCookiesAsync(UsersCookies user);
    }
}
