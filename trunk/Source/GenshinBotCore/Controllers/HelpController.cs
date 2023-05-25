using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;
using YukinoshitaBot.Extensions;

namespace GenshinBotCore.Controllers
{
    [StrictRoute(Command = "帮助", Priority = 5)]
    public class HelpController : BotControllerBase
    {

        [FriendText, GroupText]
        public void TextMsgHandler()
        {
            ReplyTextMsg(Help());
        }

        private string Help()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--------原神查询机器人--------")
              .AppendLine("**不要中括号，指令参数空格隔开**")
              .AppendLine("-----------------------------")
              .AppendLine("1. 绑定账号：[原神登录 手机号 验证码]")
              .AppendLine("   https://bbs.mihoyo.com/ys 获取验证码")
              .AppendLine("2. 基本信息：[我的原神]")
              .AppendLine("3. 状态查询：[查询原神状态]")
              .AppendLine("4. 随机表情：[随机表情]")
              .AppendLine("5. 随机表情：[随机表情 筛选条件]")
              .AppendLine("    例：[随机表情 刻晴]")
              .AppendLine("6. 骰子")
              .AppendLine("7. 代谢计算：[代谢计算 性别 体重 身高 年龄]")
              .AppendLine("8. 代谢计算运动：[代谢计算 性别 体重 身高 年龄 运动系数]")
              .AppendLine("9. 代谢计算热量差：[代谢计算 性别 体重 身高 年龄 运动系数]")
              .AppendLine("10. 代谢计算热量摄入：[代谢计算 性别 体重 身高 年龄 运动系数]")
              .AppendLine("    例：[代谢计算热量差 男 72 165 18 1.55]")
              .AppendLine("运动特别少.除了上班下班走走路.不会额外增加\n运动量的×1.2（运动系数）\n规律一周1-3次轻强度运动.比如偶尔瑜伽普拉提\n不负重的训练×1.375（运动系数）\n规律4-5次的运动.已经有运动习惯了.但每次强度\n都不会太大太多消耗×1.4625（运动系数）\n规律4-5次高强度运动.HIIT可以坚持30分钟以上\n举铁去健身房力量负重训练×1.55（运动系数)\n高强度训练强度！每天都运动可以和专业运动员\n的容量相比×1.63（运动系数）\n")
              .AppendLine("-----------------------------");

            return sb.ToString().TrimEnd('\r', '\n');
        }
    }
}
