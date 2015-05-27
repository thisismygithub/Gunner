using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Demo;

public partial class DemoCashflowPaid : System.Web.UI.Page
{
    /// <summary>
    /// 歐付寶信用卡回呼頁範例
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        #region　介接參數
        var orderNumber = Validator.GetValidatedValue(Request.Form["MerchantTradeNo"], Validator.IsInt32, "0");
        var checkMacValue = Request.Form["CheckMacValue"] ?? "";
        var hashKey = AllPayUtil.TestHashKey;
        var hashIv = AllPayUtil.TestHashIv;
        var checkstr = AllPayUtil.GetCheckMacValue(Request.Form, hashKey, hashIv);
        var rtnCode = Request.Form["RtnCode"] ?? "";//交易狀態 
        var rtnMsg = Request.Form["RtnMsg"] ?? "";//交易訊息         
        var isChecked = String.Equals(checkMacValue, checkstr, StringComparison.CurrentCultureIgnoreCase); // 檢查checksum確定AllPay傳來的資料沒有被串改.
        #endregion

        if (isChecked)
        {
            //UpdateOrder Or Do somthing// 更新訂單    
        }
        

        Response.Write(isChecked ? "1|OK" : "0|ErrorMessage");

        

        Response.End();
    }
}