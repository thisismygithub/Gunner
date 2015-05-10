using System;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;


/// <summary>
/// 基本頁面
/// </summary>
public class BasePage : Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        //DO SOMETHING IS GENERAL
    }
}

/// <summary>
/// 內網頁面
/// </summary>
public class IntranetPage : Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (IsIntranet)
        {
            //DO SOMETHING HERE    
            Response.Redirect("/cmwebapp/error/authorizedeny.aspx");
        }
        else
        {
            //DO SOMETHING HERE
            
        }
    }

    private static bool IsIntranet
    {
        get
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            //string hostName = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];//check host if need            
            if (ipAddress == "127.0.0.1" || ipAddress == "::1" || ipAddress.StartsWith("192.168.10."))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

/// <summary>
/// 會員頁面
/// </summary>
public class MemberPage : Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.OnInit(e);
        if (IsMember)
        {
            //DO SOMETHING HERE    
        }
        else
        {
            //DO SOMETHING HERE
        }
    }
    public static bool IsMember
    {
        get
        {
            //TODO:Check your member System
            var result = true;
            return result;
        }
    }
}


