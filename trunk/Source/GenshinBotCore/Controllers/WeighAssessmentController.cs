using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using YukinoshitaBot.Data.Attributes;
using YukinoshitaBot.Data.Controller;
using YukinoshitaBot.Data.Event;
using System.Security.Cryptography;

namespace GenshinBotCore.Controllers
{
    [StartRoute(Command = "代谢计算", Priority = 9)]
    public class WeighAssessmentController : BotControllerBase
    {
        [FriendText, GroupText]
        public void Counter()
        {
            //体重*10 + 身高*6.25 -年龄*5 + 5    男生
            //体重*10 + 身高*6.25 -年龄*5 - 161  女生
            var cmd = (this.Message as TextMessage)?.Content.Split(' ') ?? throw new NullReferenceException();
            double basic = 0;//基础代谢
            double dayin = 0;//每日热量摄入
            double howerreduce = 0;//减脂热量差
            double howerin = 0;//减脂热量摄入
            double weigh = 0;//体重
            double high = 0;//身高
            double old = 0;//年龄
            double counter = 0;//计算出来的值
            double sportparame = 0;
            var sex = "";
            var sb = new StringBuilder();

            #region 值转换
            if (cmd.Length < 4)
            {
                return;
            }
            sex = cmd[1];
            if (double.TryParse(cmd[2], out var value))
            {
                weigh = value;
            }
            if (double.TryParse(cmd[3], out var values))
            {
                high = values;
            }
            if (double.TryParse(cmd[4], out var valuess))
            {
                old = valuess;
            }
            if (weigh == 0 || high == 0 || old == 0)
            {
                ReplyTextMsg("输入的格式错误，请重试");
                return;
            }
            if (cmd.Length > 5 && double.TryParse(cmd[5], out var valuesss))
            {
                sportparame = valuesss;
            }
            #endregion

            basic = CounterBasic(sex, counter, weigh, high, old);
            if (basic <= 0)
            {
                return;
            }
            if (cmd[0] == "代谢计算")
            {
                ReplyTextMsg("您的基础代谢值 " + basic + " 千卡");
                return;
            }
            if (cmd[0] == "代谢计算运动")
            {
                dayin = CounterEveryDayIn(basic, sportparame);
                ReplyTextMsg("您的一天消耗为 " + dayin + " 千卡");
                return;
            }
            if (cmd[0] == "代谢计算热量差")//带三大营养换算
            {
                dayin = CounterEveryDayIn(basic, sportparame);
                howerreduce = CounterHowerReduce(dayin);
                var coins = CounterCoin(howerreduce, weigh);
                sb.AppendLine("您的减脂热量差为 " + howerreduce + " 千卡");
                sb.AppendLine("在减脂期中建议每日摄入 " + howerreduce + " 千卡");
                sb.AppendLine("您的蛋白质建议摄入热量为 " + coins[0] + " 千卡");
                sb.AppendLine("您的脂肪建议摄入量为 " + coins[1] + " 千卡");
                sb.AppendLine("您的碳水建议摄入量为 " + coins[2] + " 千卡");
                sb.AppendLine("您的蛋白质每日建议摄入克数为 " + coins[3] + " 克");
                sb.AppendLine("您的脂肪每日建议摄入克数为 " + coins[4] + " 克");
                sb.AppendLine("您的碳水每日建议摄入克数为 " + coins[5] + " 克");
                ReplyTextMsg(sb.ToString().TrimEnd('\r', '\n'));
                return;
            }
            if (cmd[0] == "代谢计算热量摄入")
            {
                dayin = CounterEveryDayIn(basic, sportparame);
                howerreduce = CounterHowerReduce(dayin);
                howerin = CounterHowerIn(dayin, howerreduce);
                var coins = CounterCoin(howerreduce, weigh);
                sb.AppendLine("您的减脂热量摄入为 " + howerin + " 千卡");
                sb.AppendLine("您的蛋白质建议摄入热量为 " + coins[0] + " 千卡");
                sb.AppendLine("您的脂肪建议摄入量为 " + coins[1] + " 千卡");
                sb.AppendLine("您的碳水建议摄入量为 " + coins[2] + " 千卡");
                sb.AppendLine("您的蛋白质每日建议摄入克数为 " + coins[3] + " 克");
                sb.AppendLine("您的脂肪每日建议摄入克数为 " + coins[4] + " 克");
                sb.AppendLine("您的碳水每日建议摄入克数为 " + coins[5] + " 克");
                ReplyTextMsg(sb.ToString().TrimEnd('\r', '\n'));
                return;
            }
        }

        /// <summary>
        /// 基础代谢
        /// </summary>
        public double CounterBasic(string sex, double counter, double weigh, double high, double old)
        {//"9. 代谢计算：[代谢计算 性别 体重 身高 年龄]"

            if (sex == "男")
            {
                counter = weigh * 10 + high * 6.25 + old * 5 + 5;
            }
            else
            {
                counter = weigh * 10 + high * 6.25 + old * 5 - 161;
            }
            return Math.Round(counter, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 每日热量摄入
        /// </summary>
        public double CounterEveryDayIn(double basic, double sportparame)
        {
            return Math.Round(basic * sportparame, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 减脂热量差
        /// </summary>
        public double CounterHowerReduce(double dayin)
        {
            return Math.Round(dayin * 0.85, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 减脂热量摄入
        /// </summary>
        public double CounterHowerIn(double dayin, double howerreduce)
        {
            return Math.Round(dayin - howerreduce, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 三大营养素
        /// </summary>
        /// <param name="howerreduce"></param>
        /// <param name="weigh"></param>
        /// <returns></returns>
        public double[] CounterCoin(double howerreduce, double weigh)
        {
            double[] coin = new double[6];
            double protein = 0;
            double fat = 0;
            double carbohydrate = 0;
            double proteing = 0;
            double fatg = 0;
            double carbohydrateg = 0;
            protein = weigh * 1.8 * 4;
            fat = weigh * 0.8 * 9;
            carbohydrate = howerreduce - protein - fat;
            proteing = protein / 4;
            fatg = fat / 9;
            carbohydrateg = carbohydrate / 4;
            coin[0] = protein;
            coin[1] = fat;
            coin[2] = carbohydrate;
            coin[3] = proteing;
            coin[4] = fatg;
            coin[5] = carbohydrateg;
            for (int i = 0; i < coin.Length; i++)
            {
                coin[i] = Math.Round(coin[i], 1, MidpointRounding.AwayFromZero);
            }
            return coin;
        }
    }
}
