using System;
using System.Collections.Generic;
using System.Linq;
using CMoney.WebBackend.WebMessageLog;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using CMoney.WebBackend.WebFunction;

namespace CMoney.WebFrontend.GeneralTools
{
    public class Util
    {
        ///// <summary>
        ///// 三天
        ///// </summary>
        //private const int LOG_TIMEOUT = 259200;

        ///// <summary>
        ///// 最大Log數
        ///// </summary>
        //private const int LOG_MAX_LENGTH = 1000;

        #region web message log

        //private static WebMessageLogManager _webMessage;
        //public static WebMessageLogManager WebMessage
        //{
        //    get
        //    {
        //        if (_webMessage == null)
        //        {
        //            _webMessage = new WebMessageLogManager(LOG_TIMEOUT, LOG_MAX_LENGTH);
        //        }
        //        return _webMessage;
        //    }
        //}

        #region AddMessageLog 新增一筆 web log

        ///// <summary>
        ///// 新增一筆 web log
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="startTime"></param>
        //public static void AddMessageLog(string message, int startTime)
        //{
        //    int errorCode;
        //    string errorMessage;
        //    var sessionId = GetLogSessionId();

        //    WebMessage.AddMessageLog(sessionId, message, startTime, out errorCode, out errorMessage);
        //}
        #endregion

        #region GetLogSessionId 記錄 web log 的SessionId

        ///// <summary>
        ///// 記錄 web log 的SessionId
        ///// </summary>
        ///// <returns></returns>
        //public static Guid GetLogSessionId()
        //{
        //    #region 判斷Session["LogSessionId"]有沒有值，有值則轉換成Guid, 無值則建一個新Guid
        //    Guid sessionId;
        //    if (HttpContext.Current.Session["LogSessionId"] == null)
        //    {
        //        sessionId = Guid.NewGuid();
        //        HttpContext.Current.Session["LogSessionId"] = sessionId;
        //    }
        //    else
        //    {
        //        Guid.TryParse(HttpContext.Current.Session["LogSessionId"].ToString(), out sessionId);
        //        if (sessionId == Guid.Empty)
        //        {
        //            sessionId = Guid.NewGuid();
        //            HttpContext.Current.Session["LogSessionId"] = sessionId;
        //        }
        //    }
        //    #endregion
        //    return sessionId;
        //}


        #endregion

        #endregion

        /// <summary>
        /// 本機Urlhost
        /// </summary>
        /// <returns></returns>
        public static string LocalBaseUrl()
        {
            try
            {
                return "http://127.0.0.1/";
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetBaseUrl()
        {
            try
            {
                return string.Format("http://{0}/", HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetSslBaseUrl()
        {
            try
            {
                return string.Format("https://{0}/", HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetLinkElement(string path, string media = "")
        {
            if (string.IsNullOrEmpty(media))
            {
                return string.Format("<link href=\"{0}?_={1}\" rel=\"stylesheet\" type=\"text/css\" />", path, GetLastWriteTime(path));
            }
            else
            {
                return string.Format("<link href=\"{0}?_={1}\" rel=\"stylesheet\" type=\"text/css\" media=\"{2}\" />", path, GetLastWriteTime(path), media);
            }
        }

        public static string GetScriptElement(string path)
        {
            return string.Format("<script src=\"{0}?_={1}\" type=\"text/javascript\"></script>", path, GetLastWriteTime(path));
        }

        public static long GetLastWriteTime(string path)
        {
            var lastWriteTime = File.GetLastWriteTime(HttpContext.Current.Server.MapPath(path));

            return lastWriteTime.Ticks;
        }

        public static string GetRandomAlphanumericChars(int length)
        {
            var chars = "ACDEFGHJKLMNPQRSTUVWXYZ2345679";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return result;
        }

        public static bool IsFromAppViewer()
        {
            string userAgent = HttpContext.Current.Request.UserAgent;

            if (userAgent != null && userAgent.IndexOf("AppViewer") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 移除HTML標籤。
        /// </summary>
        public static string StripHtmlTags(string source)
        {
            var Result = string.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                char[] array = new char[source.Length];
                int arrayIndex = 0;
                bool inside = false;

                for (int i = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    if (let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (let == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                }
                Result = new string(array, 0, arrayIndex);
            }
            return Result;
        }

        /// <summary>
        /// Truncate text to a certain length.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <param name="showToolTip"></param>
        /// <returns></returns>
        public static string TruncateText(string text, int maxLength, bool showToolTip, bool showEllipsis = true)
        {
            string truncatedText = string.Empty;

            if (text == null)
            {
                text = string.Empty;
            }

            if (text.Length <= maxLength)
            {
                truncatedText = text;
            }
            else
            {
                truncatedText = text.Substring(0, maxLength);

                if (showEllipsis)
                {
                    truncatedText += "...";
                }
            }

            if (showToolTip)
            {
                return string.Format("<div style=\"display:inline;\" title=\"{0}\">{1}</div>", text, truncatedText);
            }
            else
            {
                return truncatedText;
            }
        }

        public static string TruncateTextByCss(string text, int maxLength, bool showToolTip)
        {
            if (showToolTip)
            {
                return string.Format("<div style=\"overflow : hidden; text-overflow : ellipsis; white-space : nowrap; width : {0}px;\" title=\"{1}\">{1}</div>", maxLength, text);
            }
            else
            {
                return string.Format("<div style=\"overflow : hidden; text-overflow : ellipsis; white-space : nowrap; width : {0}px;\">{1}</div>", maxLength, text);
            }
        }

        public static string GetAnonymisedEmailAddress(string emailAddress)
        {
            string str = Validator.GetValidatedValue(emailAddress, Validator.IsEmailAddress, null);
            if (str == null)
            {
                return emailAddress;
            }

            var anonymisedEmailAddress = str.Substring(0, str.IndexOf('@') / 2 + str.IndexOf('@') % 2) +
                                        string.Empty.PadRight(str.IndexOf('@') / 2, '*') +
                                        str.Substring(str.IndexOf('@'));

            return anonymisedEmailAddress;
        }

        public static string GetIpAddress()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-Forwarded-For"]))
            {
                return HttpContext.Current.Request.UserHostAddress ?? "";
            }
            else
            {
                string xForwardedFor = HttpContext.Current.Request.Headers["X-Forwarded-For"];
                string[] ipAddressList = xForwardedFor.Split(',');
                if (ipAddressList.Count() > 0)
                {
                    return ipAddressList[0].Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string UpdateInvoiceInfo(string invoiceInfo, string key, string value)
        {
            var dicInvoiceInfo = new Dictionary<string, string>();
            string result = string.Empty;
            if (!string.IsNullOrEmpty(invoiceInfo))
            {
                List<string> tmpListInvoiceInfo = invoiceInfo.Split(new string[] { "<|" }, StringSplitOptions.None).ToList();
                foreach (string tmpII in tmpListInvoiceInfo)
                {
                    string[] tmpI = tmpII.Split(new string[] { "=>" }, StringSplitOptions.None);
                    if (tmpI.Length == 2)
                    {
                        dicInvoiceInfo.Add(tmpI[0].ToString(), tmpI[1].ToString());
                    }
                }
                dicInvoiceInfo[key] = value;
                var tmpIII = new List<string>();
                foreach (var dic in dicInvoiceInfo)
                {
                    tmpIII.Add(string.Format("{0}=>{1}", dic.Key, dic.Value));
                }
                result = string.Join("<|", tmpIII);
            }

            return result;
        }
        
        public static bool IsFromMobile()
        {
            var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var result = b.IsMatch(u) || v.IsMatch(u.Substring(0, 4));

            return result;
        }

        public static string GetAbsoluteCMoneyUrl(string cmoneyUrl)
        {
            if (string.IsNullOrEmpty(cmoneyUrl))
            {
                cmoneyUrl = string.Empty;
            }
            else if (cmoneyUrl.StartsWith("./") || cmoneyUrl.StartsWith("../"))
            {
                cmoneyUrl = GetBaseUrl() + cmoneyUrl;
            }
            else if (cmoneyUrl.StartsWith("/"))
            {
                cmoneyUrl = GetBaseUrl().TrimEnd('/') + cmoneyUrl;
            }

            return cmoneyUrl;
        }

        public static string GetCMoneySite(string cmoneyUrl)
        {
            var siteName = cmoneyUrl.Substring(cmoneyUrl.IndexOf("cmoney.tw") + 10);
            if (siteName.IndexOf("/") != -1)
            {
                siteName = siteName.Substring(0, siteName.IndexOf("/"));
            }

            return siteName;
        }

        #region　取得經過時間

        /// <summary>
        /// 取得經過時間
        /// </summary>
        /// <param name="dateTime">時間</param>
        /// <returns></returns>
        public static double GetTime(DateTime dateTime)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = dateTime.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return ts.TotalMilliseconds;
        }
        #endregion
    }
}
