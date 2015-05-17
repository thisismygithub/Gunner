<%@ WebHandler Language="C#" Class="Demo.DemoHandler" %>


using System.Web;

namespace Demo
{
    public class DemoHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Process.Main();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }

}
