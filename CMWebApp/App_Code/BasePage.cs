using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;


/// <summary>
/// 基本頁面
/// </summary>
public class BasePage : Page
{
    protected Dictionary<string, object> PageDataDic = new Dictionary<string, object>();
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
        if (Util.IsIntranet)
        {
            //DO SOMETHING HERE    
            Response.Redirect("/cmwebapp/error/authorizedeny.aspx");
        }
        else
        {
            //DO SOMETHING HERE
            
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


