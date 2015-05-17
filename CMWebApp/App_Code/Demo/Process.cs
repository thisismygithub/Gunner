
// Demo Namespace底下，只放著demo用的class 

using System.Collections.Generic;
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
        public static void Main()
        {
            var context = HttpContext.Current;
            string action = context.Request.Params["action"] ?? string.Empty;
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
            var data = GetData.PagerData(physicalPath);
            var result = data;
            return result;
        }
    }

    /// <summary>
    /// 取資料
    /// </summary>
    public static class GetData
    {

        internal static List<PagerData> PagerData(string jsonFilePath)
        {
            var content = IO.GetFileContent(jsonFilePath);
            var result = (List<PagerData>)DataConverter.Deserialize(content, typeof(List<PagerData>));
            return result;
        }
    }

    #region DEMO Data Class for Serialize and Deserialize

    public class PagerData
    {
        public uint PostId { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }

    #endregion
}