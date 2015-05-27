using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Demo
{
    /// <summary>
    /// 歐付寶的專屬類別
    /// 參考:https://www.allpay.com.tw/Content/files/allpay_011.pdf
    /// </summary>
    public class AllPayUtil
    {
        public static string TestMerchantID = "2000132";
        public static string TestHashKey = "5294y06JbISpM5x9";
        public static string TestHashIv = "v77hoKGq4kWxNNIS";

        /// <summary>
        /// 在與歐付寶進行資料傳遞時，除了 CheckMacValue 參數外，其餘所有參數皆需要加入檢查碼的
        /// 檢核機制。
        /// 檢碼核制如下:
        /// 將傳遞參數依照第一個英文字母，由 A 到 Z 的順序來排序(遇到第一個英名字母相同時，以第二
        /// 個英名字母來比較，以此類推) ，並且以& 方式將所有參數串連後， 再將參數最前面加上 HashKey、
        /// 最後面加上 HashIV，並將整串字串進行 URL encode 後再轉為小寫，再以 MD5 加密方式來產生
        /// CheckMacValue。
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="hashKey"></param>
        /// <param name="hashIv"></param>
        /// <returns></returns>
        public static string GetCheckMacValue(NameValueCollection parameters, string hashKey, string hashIv)
        {

            #region　除了 CheckMacValue 參數外, 將傳遞參數依照第一個英文字母，由 A 到 Z 的順序來排序
            var parametersString = string.Empty;
            var sortedDataset = parameters.AllKeys.OrderBy(k => k);
            foreach (var item in sortedDataset)
            {
                if (item != "CheckMacValue")
                {
                    parametersString += item + "=" + parameters[item];
                    if (sortedDataset.Last() != item)
                    {
                        parametersString += "&";
                    }
                }
            }
            #endregion
            var checkstr = string.Format(@"HashKey={0}&{1}&HashIV={2}", hashKey, parametersString, hashIv);
            checkstr = HttpUtility.UrlEncode(checkstr);
            checkstr = checkstr.ToLower();
            //checkstr = FormsAuthentication.HashPasswordForStoringInConfigFile(checkstr, "md5");


            return Md5Hash(checkstr);
        }


        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}