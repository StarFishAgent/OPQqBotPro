using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukinoshitaBot.Entities;

namespace YukinoshitaBot.Services.UserManager
{
    public interface IUserManager
    {
        User? GetUserById(int id);
        User? GetUserByQQ(long qqId);
        User? GetUserByMihoyoId(string mihoyoId);
        User? GetUserByGenshinUid(string genshinUid);
        Task<User?> UpdateUserAsync(User user);
        Task<UsersComment?> UpdateUserCommentAsync(UsersComment user);
    }
}
