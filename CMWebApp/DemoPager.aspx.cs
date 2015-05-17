using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoPager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int page = 0 ;
        int.TryParse(Request.QueryString["page"],out page);
        if (page<1)
        {
            page = 1;
        }
    }

    private static string PagerBuilder(int page, int totalPage)
    {
        var result = "<span class='page'>";

        // 顯示的頁數範圍
        var range = 3;

        // 顯示當前分頁鄰近的分頁頁數            
        result += "<a href='javascript:_PageObject.GetAds(1);' >第一頁</a>";
        for (int x = ((page - range) - 1); x < ((page + range) + 1); x++)
        {
            // 如果這是一個正確的頁數...
            if ((x > 0) && (x <= totalPage))
            {
                // 如果這一頁等於當前頁數...
                if (x == page)
                {
                    // 不使用連結, 但用高亮度顯示
                    //result += "<a href='?p="+x+"' class='pageNow'>"+x+"</a>";                                
                    result += "<a href='#' page=" + x + " class='pageNow'>" + x + "</a>";
                    // 如果這一頁不是當前頁數...
                }
                else
                {
                    // 顯示連結
                    result += "<a href='javascript:_PageObject.GetAds(" + x + ");'>" + x + "</a> ";
                    //result += "<a href='#'>" + x + "</a> ";
                } // end else
            } // end if
        } // end for

        result += "<a href='javascript:_PageObject.GetAds(" + totalPage + ")' >最後一頁</a>";
        result += "</span>";
        return result;
    }
}