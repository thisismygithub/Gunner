
// Demo Namespace底下，只放著demo用的class 

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Demo
{
    public class Process
    {
        #region 組檔案路徑

        private static readonly string AppPath = HttpContext.Current.Request.ApplicationPath;
        static string physicalPath = string.Format(@"{0}\App_Code\Demo\pagerdata.json", HttpContext.Current.Request.MapPath(AppPath));
        #endregion

        protected static NameValueCollection ReqParams = null; 
        public static void Main()
        {
            ReqParams = HttpContext.Current.Request.Params;
            var context = HttpContext.Current;
            string action = ReqParams["action"] ?? string.Empty;
            object response = null;
            switch (action.ToLower())
            {
                case "getpagerdata":
                    response = GetPagerData();                    
                    break;
                default:
                    break;
            }
            context.Response.Write(DataConverter.Serialize(response));
            context.Response.End();
        }

        private static object GetPagerData()
        {
            var page = new Pager(physicalPath) ;
            
            var targetPage = 1;
            int.TryParse(ReqParams["page"] ?? "1", out targetPage);

            return page.GetData(targetPage, 18);
        }
    }


}