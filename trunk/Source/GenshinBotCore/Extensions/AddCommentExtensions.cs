using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using PuppeteerSharp;

using Spectre.Console;

namespace GenshinBotCore.Extensions
{
    public static class AddCommentExtensions
    {
        public static async Task LoadAll(string VideoUrl, string QQNum)
        {
            CreateDire();
            await CheckDownloadBrowser();
            await CreateBrowser();
            //await LoadAddComment(await IniPage(VideoUrl, QQNum));
        }

        #region 必要参数
        /// <summary>
        /// 秒表
        /// </summary>
        public static Stopwatch sw { get; set; } = new Stopwatch();

        /// <summary>
        /// 推送+推送服务
        /// </summary>
        public static PushPlus pushPlus { get; set; }

        /// <summary>
        /// 浏览器实例
        /// </summary>
        public static Browser browser { get; set; }

        /// <summary>
        /// 存储Cookies
        /// </summary>
        public static CookieParam[] cookies { get; set; }
        #endregion

        #region 业务逻辑
        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void CreateDire()
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "QRPic") == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "QRPic");
            }
        }
        /// <summary>
        /// 检查下载浏览器
        /// </summary>
        /// <returns></returns>
        public async static Task<bool> CheckDownloadBrowser()
        {
            Console.WriteLine("----------------------------------------------------------------------");
            var br = new BrowserFetcher();
            var ExecutablePath = await br.GetRevisionInfoAsync();

            var Downloaded = ExecutablePath.Downloaded;
            Console.WriteLine($"检查当前是否下载浏览器:{(Downloaded == true ? "已下载" : "未下载")}");

            if (!Downloaded)
            {
                Console.WriteLine($"准备开始下载浏览器");
                Console.WriteLine($"平台:{ExecutablePath.Platform}");
                Console.WriteLine($"当前默认选择浏览器:{br.Product}");
                Console.WriteLine($"默认下载浏览器版本:{ExecutablePath.Revision}");

                /*Console.WriteLine($"Local:{ExecutablePath.Local}");
                Console.WriteLine($"文件夹路径:{ExecutablePath.FolderPath}");
                Console.WriteLine($"执行文件路径:{ExecutablePath.ExecutablePath}");
                Console.WriteLine($"手动下载地址:{ExecutablePath.Url}");*/

                Console.WriteLine("--------------------");
                AnsiConsole.MarkupLine("[green]开始[/]下载浏览器");
                br.DownloadProgressChanged += (a, b) => //下载进度条事件
                {
                    if (b.ProgressPercentage >= 100)
                    {
                        AnsiConsole.MarkupLine("下载浏览器[red]完毕[/]");
                    }
                };

                await br.DownloadAsync();
            }
            else
            {
                Console.WriteLine($"平台:{ExecutablePath.Platform}");
                Console.WriteLine($"当前浏览器:{br.Product}");
                Console.WriteLine($"当前浏览器版本:{ExecutablePath.Revision}");
            }

            Console.WriteLine("----------------------------------------------------------------------");
            return true;
        }
        /// <summary>
        /// 创建浏览器实例
        /// </summary>
        /// <returns></returns>
        public async static Task<bool> CreateBrowser()
        {
            #region 本地启动
            var LaunchOptions = new LaunchOptions
            {
                ExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                Args = new string[]
            {
                                                       LaunchOptionsArgs.audioEnabled
            },
                IgnoreDefaultArgs = true, //是否开启忽略默认参数
                IgnoredDefaultArgs = new string[]
            {
                                                       IgnoredOptionsArgs.disableWebdriver属性
            },
                Headless = false, //是否在无头模式下运行浏览器 默认true 无头
                                  //Timeout = 1000 * 60,//等待浏览器实例启动的最长时间 默认30秒
                Product = Product.Chrome,//浏览器使用哪个 默认Chrome
                                         //SlowMo = 1000, //自动操作的速度非常快，以至于看不清楚元素的动作，为了方便调试，可以用 slowMo 参数放慢操作，单位 ms：
                DefaultViewport = new ViewPortOptions() //设置默认视图
                {
                    Width = 1920,
                    Height = 1050,
                    IsMobile = true,//是否手机
                },
                IgnoreHTTPSErrors = false,//导航期间是否忽略HTTPS错误。默认为false
                Devtools = false,//是否为每个选项卡自动打开DevTools面板。如果此选项为true，则无头选项将设置为false。
            };

            //var LaunchOptions = new LaunchOptions
            //{
            //    ExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
            //    Headless = false, //是否在无头模式下运行浏览器 默认true 无头
            //    IgnoreDefaultArgs = true, //是否开启忽略默认参数
            //    IgnoredDefaultArgs = new string[]
            //            {
            //                                           IgnoredOptionsArgs.disableWebdriver属性
            //            },
            //};
            browser = await Puppeteer.LaunchAsync(LaunchOptions);
            #endregion

            #region 远程链接
            //启动参数 --remote-debugging-address=0.0.0.0 --remote-debugging-port=4079
            // 检查你的浏览器远程访问是否开启:http://127.0.0.1:4079/ edge://inspect/#devices
            //var ConnectOptions = new ConnectOptions()
            //{
            //    BrowserURL = $"http://127.0.0.1:80",
            //    DefaultViewport = new ViewPortOptions() //设置默认视图
            //    {
            //        Width = 1920,
            //        Height = 1028
            //    },
            //};
            //browser = await Puppeteer.ConnectAsync(ConnectOptions);
            #endregion

            return true;
        }
        //AnsiConsole.WriteLine(browser.WebSocketEndpoint); //输出ws终结点
        public static async Task<Page> IniPage(string VideoUrl, string QQnum)
        {
            var page = await browser.NewPageAsync();
            await page.GoToAsync(VideoUrl);
            await page.ClickAsync("#app > div > div.layout-main > div > div.main-container > div.new-guide > div.jump-btn");
            await page.WaitForTimeoutAsync(200);
            await page.ClickAsync("#app > div > div.layout-main > div > div.main-container > div.volume-select > div > div.btn-right");
            await page.WaitForTimeoutAsync(200);
            await page.ClickAsync("#app > div > div.layout-main > div > div.nav > div.nav-right > div");
            await page.WaitForTimeoutAsync(3000);
            await page.ScreenshotAsync(AppDomain.CurrentDomain.BaseDirectory + "QRPic\\" + QQnum + ".png");
            return page;
        }
        public static async Task LoadAddComment(Page page, int CookiesCount)
        {
            if (await IsLogin(page, CookiesCount))
            {
                await page.ClickAsync("#app > div > div.layout-main > div > div.main-container > div.swiper-container.swiper-container-initialized.swiper-container-vertical.swiper-container-pointer-events > div > div.swiper-slide.swiper-slide-active > div > div.content-mask > div > div.feed-action > div.comment-content > div.icon");
                await AddComment(page);
                await browser.CloseAsync();
            }
            else
            {
                await browser.CloseAsync();
            }
        }

        /// <summary>
        /// 循环判断是否登录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="CookiesCount"></param>
        /// <returns></returns>
        public static async Task<string> ForeachIsLogin(Page page, int CookiesCount)
        {
            int i = 0;
            while (await IsLogin(page, CookiesCount) == false)
            {
                if (i >= 25)
                {
                    break;
                }
                await page.WaitForTimeoutAsync(1000);
                i++;
            }
            if (i >= 25 && await IsLogin(page, CookiesCount))
            {
                return "登录成功，请稍等";
            }
            else if (i >= 25)
            {
                return "登录未成功，请重新登录";
            }
            return "登录成功，请稍等";
        }

        /// <summary>
        /// 刷评论
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task AddComment(Page page)
        {
            for (int i = 0; i < 10; i++)
            {
                char[] guid = Guid.NewGuid().ToString().ToCharArray();
                foreach (var item in guid)
                {
                    await page.WaitForTimeoutAsync(2000);
                    await page.TypeAsync("#app > div > div.layout-drawer > div > div.comment-post > div > div.comment-input-wrapper > div.comment-input", item.ToString());
                    await page.WaitForTimeoutAsync(2000);
                    await page.ClickAsync("#app > div > div.layout-drawer > div > div.comment-post > div > div.comment-button.abled");
                }
            }
        }

        /// <summary>
        /// 获取页面cookies数量
        /// </summary>
        /// <param name="page">网页实例</param>
        /// <returns></returns>
        public static async Task<int> GetPageCookiesCount(Page page)
        {
            var cookieParams = await page.GetCookiesAsync();
            return cookieParams.Count();
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <param name="page">网页实例</param>
        /// <param name="CookiesCount">cookies数量</param>
        /// <returns></returns>
        public static async Task<bool> IsLogin(Page page, int CookiesCount)
        {
            return CookiesCount < await GetPageCookiesCount(page);
        }
        #endregion

        #region 其他类
        /// <summary>
        /// 列举了一些ChromiumCommand  https://peter.sh/experiments/chromium-command-line-switches/ 这里基本列举了所有命令
        /// </summary>
        public static class LaunchOptionsArgs
        {
            /// <summary>
            /// 禁用拓展
            /// </summary>
            public const string disableExtensions = "--disable-extensions";

            /// <summary>
            /// 隐蔽滑动条
            /// </summary>
            public const string hideScrollbars = "--hide-scrollbars";

            /// <summary>
            /// 禁用gpu
            /// </summary>
            public const string disableGpu = "--disable-gpu";

            /// <summary>
            /// 不加载图片, 提升速度，但无法显示二维码
            /// </summary>
            public const string imagesEnabled = "blink-settings=imagesEnabled=true'";

            /// <summary>
            /// 关闭声音
            /// </summary>
            public const string audioEnabled = "--mute-audio";

            /// <summary>
            /// 开启时最大化
            /// </summary>
            public const string startMaximized = "--start-maximized";

            /// <summary>
            /// 设置远程调试地址
            /// </summary>
            public const string remoteDebuggingAddress = "--remote-debugging-address=0.0.0.0";

            /// <summary>
            /// 设置远程调试端口 默认端口8888
            /// </summary>
            public const string remoteDebuggingPort = "--remote-debugging-port=8888";

            /// <summary>
            /// 设置浏览器窗口大小 1000,500
            /// </summary>
            public const string browerWindowSize = "--window-size=1000,500";
        }

        public static class IgnoredOptionsArgs
        {
            /// <summary>
            /// 防爬虫检测 去掉navigator.webdriver属性
            /// </summary>
            public const string disableWebdriver属性 = "--enable-automation";
        }

        /// <summary>
        /// IOS Bark
        /// </summary>
        public class BarkPush
        {
            /// <summary>
            /// 
            /// </summary>
            public string barkServerAddres { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="barkServerAddres"></param>
            public BarkPush(string barkServerAddres)
            {
                this.barkServerAddres = barkServerAddres;
            }

            /// <summary>
            /// 发送文本消息
            /// </summary>
            /// <param name="url"></param>
            /// <param name="text">内容</param>
            /// <returns></returns>
            public async Task<JObject> SendTextMsg(string text, Dictionary<BarkParameter, string> Params = null)
            {
                JObject Resutls = null;

                var Client = new HttpClient();
                Client.Timeout = new TimeSpan(0, 0, 5);
                try
                {
                    var url = $"{barkServerAddres}{text}?";
                    if (Params != null && Params.Count > 0)
                    {
                        foreach (var item in Params)
                        {
                            url += $"{item.Key}={item.Value}&";
                        }
                    }

                    var Response = await Client.GetAsync(url);
                    if (Response != null)
                    {
                        if (Response.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new Exception("BarkPush SendTextMsg Error" + await Response.Content.ReadAsStringAsync());
                        Resutls = JObject.Parse(await Response.Content.ReadAsStringAsync());
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Client.Dispose();
                }

                return Resutls;
            }

            /// <summary>
            /// 发送文本消息
            /// </summary>
            /// <param name="url"></param>
            /// <param name="text">内容</param>
            /// <param name="tittle">标题</param>
            /// <returns></returns>
            public async Task<JObject> SendTextMsg(string text, string tittle, Dictionary<BarkParameter, string> Params = null)
            {
                JObject Resutls = null;

                var Client = new HttpClient();
                Client.Timeout = new TimeSpan(0, 0, 5);
                try
                {
                    var url = $"{barkServerAddres}{tittle}/{text}?";
                    if (Params != null && Params.Count > 0)
                    {
                        foreach (var item in Params)
                        {
                            url += $"{item.Key}={item.Value}&";
                        }
                    }
                    var Response = await Client.GetAsync(url);

                    if (Response != null)
                    {
                        if (Response.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new Exception("BarkPush SendTextMsg Error" + await Response.Content.ReadAsStringAsync());
                        Resutls = JObject.Parse(await Response.Content.ReadAsStringAsync());
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Client.Dispose();
                }

                return Resutls;
            }
        }

        /// <summary>
        /// Bark可选参数
        /// </summary>
        public enum BarkParameter
        {
            /// <summary>
            /// 铃声 BarkSound
            /// </summary>
            sound,

            /// <summary>
            /// 是否保存消息 值为1自动保存
            /// </summary>
            isArchive,

            /// <summary>
            /// 自定义推送图标（需iOS15或以上）icon地址
            /// </summary>
            icon,

            /// <summary>
            /// 推送消息分组
            /// </summary>
            group,

            /// <summary>
            /// 跳转地址
            /// </summary>
            url,

            /// <summary>
            /// 复制内容
            /// </summary>
            copy,

            /// <summary>
            /// 角标
            /// </summary>
            badge,
        }

        /// <summary>
        /// 推送铃声
        /// </summary>
        public enum BarkSound
        {
            alarm,
            anticipate,
            bell,
            birdsong,
            bloom,
            calypso,
            chime,
            choo,
            descent,
            electronic,
            fanfare,
            glass,
            healthnotification,
            horn,
            ladder,
            mailsent,
            minuet,
            multiwayinvitation,
            newmail,
            newsflash,
            noir,
            paymentsuccess,
            shake,
            sherwoodforest,
            silence,
            spell,
            suspense,
            telegraph,
            tiptoes,
            typewriters,
            update,
        }

        /// <summary>
        /// PushPlus 推送加
        /// </summary>
        public class PushPlus
        {
            /// <summary>
            /// 
            /// </summary>
            public string token { get; }

            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; } = "http://www.pushplus.plus/send";

            /// <summary>
            /// 
            /// </summary>
            /// <param name="token"></param>
            public PushPlus(string token)
            {
                this.token = token;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public async Task<JObject> SendMsg(string text, PushPlusTemplate template = PushPlusTemplate.html)
            {
                JObject Resutls = null;

                var Client = new HttpClient();
                Client.Timeout = new TimeSpan(0, 0, 5);
                try
                {
                    HttpContent content = new StringContent(new PushPlusModel()
                    {
                        token = this.token,
                        content = text,
                        template = template.ToString()
                    }.ToString());
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var Response = await Client.PostAsync($"{url}", content);
                    if (Response != null)
                    {
                        Resutls = JObject.Parse(await Response.Content.ReadAsStringAsync());
                        if ((string)Resutls["msg"] != "请求成功")
                            throw new Exception("PushPlus SendMsg Error" + (string)Resutls["msg"]);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Client.Dispose();
                }

                return Resutls;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            /// <param name="tittle"></param>
            /// <returns></returns>
            public async Task<JObject> SendMsg(string text, string title, PushPlusTemplate template = PushPlusTemplate.html)
            {
                JObject Resutls = null;

                var Client = new HttpClient();
                Client.Timeout = new TimeSpan(0, 0, 5);
                try
                {
                    var body = new StringContent(new PushPlusModel()
                    {
                        token = this.token,
                        title = title,
                        content = text,
                        template = template.ToString()
                    }.ToString());
                    HttpContent content = body;
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var Response = await Client.PostAsync($"{url}", content);
                    if (Response != null)
                    {
                        Resutls = JObject.Parse(await Response.Content.ReadAsStringAsync());
                        if ((string)Resutls["msg"] != "请求成功")
                            throw new Exception("PushPlus SendMsg Error" + (string)Resutls["msg"]);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Client.Dispose();
                }

                return Resutls;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            /// <param name="tittle"></param>
            /// <param name="template"></param>
            /// <returns></returns>
            /*  public async Task<JObject> SendChannelMsg(string text, string tittle, PushPlusTemplate template = PushPlusTemplate.html, PushPlusChannel channel= PushPlusChannel.wechat)
              {
                  JObject Resutls = null;

                  var Client = new HttpClient();
                  Client.Timeout = new TimeSpan(0, 0, 5);
                  try
                  {
                      HttpContent content = new StringContent(new PushPlusModel()
                      {
                          token = this.token,
                          tittle = tittle,
                          content = text,
                          template = template.ToString()
                      }.ToString());
                      content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                      var Response = await Client.PostAsync($"{url}", content);
                      if (Response != null)
                      {
                          Resutls = JObject.Parse(await Response.Content.ReadAsStringAsync());
                          if ((string)Resutls["msg"] != "请求成功")
                              throw new Exception("PushPlus SendMsg Error" + (string)Resutls["msg"]);
                      }
                  }
                  catch
                  {
                      throw;
                  }
                  finally
                  {
                      Client.Dispose();
                  }

                  return Resutls;
              }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public enum PushPlusTemplate
        {
            /// <summary>
            /// html
            /// </summary>
            html,
            /// <summary>
            /// 纯文本
            /// </summary>
            txt,
            /// <summary>
            /// json
            /// </summary>
            json,
            /// <summary>
            /// markDown
            /// </summary>
            markdown,
            /// <summary>
            /// 阿里云报警模板
            /// </summary>
            cloudMonitor
        }

        /// <summary>
        /// 发送参数模型
        /// </summary>
        public class PushPlusModel
        {
            public string token { get; set; }
            public string title { get; set; } = "";
            public string content { get; set; }
            public string template { get; set; }
            public string channel { get; set; } = "";
            public string webhook { get; set; } = "";

            public override string ToString()
            {
                var json = new JObject();
                json.Add("token", token);
                json.Add("title", title);
                json.Add("content", content);
                json.Add("template", template);
                if (!string.IsNullOrWhiteSpace(channel))
                {
                    json.Add("template", template);
                    json.Add("webhook", webhook);
                }

                return json.ToString();
            }
        }

        /// <summary>
        /// 发送渠道枚举
        /// </summary>
        public enum PushPlusChannel
        {
            /// <summary>
            /// 微信公众号,默认发送渠道
            /// </summary>
            wechat,

            /// <summary>
            /// 第三方webhook服务；企业微信机器人、钉钉机器人、飞书机器人
            /// </summary>
            webhook,

            /// <summary>
            /// 企业微信应用
            /// </summary>
            cp,

            /// <summary>
            /// 邮件
            /// </summary>
            mail,

            /// <summary>
            /// 短信，未开放使用
            /// </summary>
            //sms
        }
        #endregion

    }
}
