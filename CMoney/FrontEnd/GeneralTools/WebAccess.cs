using System;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;

namespace CMoney.WebFrontend.GeneralTools
{
    public class WebAccess
    {
        public string Url { get; set; }
        public string FormName { get; set; }
        private NameValueCollection _dataSet = new NameValueCollection();

        /// <summary>
        /// Creates a new instance of the WebAccess class
        /// </summary>
        public WebAccess()
        {
            Url = string.Empty;
            FormName = "form1";
        }

        public WebAccess(string url, string formName = "form1")
        {
            Url = url;
            FormName = formName;
        }

        /// <summary>
        /// Adds data to be submitted.
        /// </summary>
        public void AddData(string name, string value)
        {
            _dataSet.Add(name, value);
        }

        /// <summary>
        /// Load page content.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>Page content in HTML</returns>
        public string LoadAtServer(int timeout = 120000)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest hwRequest = (HttpWebRequest)HttpWebRequest.Create(Url);
                hwRequest.Timeout = timeout;
                HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                Stream myStream = hwResponse.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                result = strBuilder.ToString();
            }
            catch
            {
                // CMoney.Logger.Log(ex); might not be set up, so do nothing.
            }

            return result;
        }

        /// <summary>
        /// Load page content.
        /// </summary>
        /// <param name="url"> </param>
        /// <param name="timeout"></param>
        /// <returns>Page content in HTML</returns>
        public static string LoadAtServer(string url, int timeout = 120000)
        {
            string result = string.Empty;

            try
            {
                var hwRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                hwRequest.Timeout = timeout;
                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                var myStream = hwResponse.GetResponseStream();
                var sr = new StreamReader(myStream, Encoding.UTF8);
                var strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                result = strBuilder.ToString();
            }
            catch
            {
                // CMoney.Logger.Log(ex); might not be set up, so do nothing.
            }

            return result;
        }

        /// <summary>
        /// Submit data at client-side.
        /// The target URL can be a relative URL.
        /// </summary>
        public void SubmitAtClient()
        {
            var context = HttpContext.Current;
            context.Response.Clear();
            context.Response.Write("<html><head>");
            context.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            context.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" target=\"_self\">", FormName, "POST", Url));
            for (int i = 0; i < _dataSet.Keys.Count; i++)
            {
                context.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", HttpUtility.HtmlEncode(_dataSet.Keys[i]), HttpUtility.HtmlEncode(_dataSet[_dataSet.Keys[i]])));
            }
            context.Response.Write("</form>");
            context.Response.Write("</body></html>");
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            context.Response.End();
        }

        /// <summary>
        /// Submit data at server-side.
        /// The target URL must be an absolute URL that starts with http:// (https might have certificate problem).
        /// </summary>
        public string SubmitAtServer()
        {
            var result = string.Empty;

            try
            {
                HttpWebRequest hwRequest = (HttpWebRequest)WebRequest.Create(Url);
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";

                string parameters = GenParameters();

                byte[] buffer = Encoding.UTF8.GetBytes(parameters);
                hwRequest.ContentLength = buffer.Length;
                var postData = hwRequest.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();

                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                var stream = hwResponse.GetResponseStream();
                var streamReader = new StreamReader(stream);
                result = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                // CMoney.Logger.Log(ex);
                result = ex.ToString();
            }

            return result;
        }

        private string GenParameters()
        {
            var result = string.Empty;
            //歐付寶檢查碼機制:將傳遞參數依照第一個英文字母，由 A 到 Z 的順序來排序(遇到第一個英名字母相同時，以第二
            //個英名字母來比較，以此類推) ，並且以&方式將所有參數串連
            var sortedDataset = _dataSet.AllKeys.OrderBy(k => k);
            foreach (var item in sortedDataset)
            {
                result += string.Format("{0}={1}", item, _dataSet[item]);
                if (sortedDataset.Last() != item)
                {
                    result += "&";
                }
            }

            return result;
        }

        public NameValueCollection GetUrlParameters()
        {
            return _dataSet;
        }
    }
}
