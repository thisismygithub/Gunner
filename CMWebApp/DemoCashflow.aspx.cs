using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Demo;

public partial class DemoCashflow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        PayByCreditCardAllPay("MAR-LO-9999-001", 100, "m test");
    }

    /// <summary>
    /// 歐付寶-信用卡付款
    /// </summary>    
    /// <param name="merchantTradeNo">廠商訂單編號</param>
    /// <param name="amount">金額</param>
    /// <param name="tradeDesc">交易描述</param>
    public static void PayByCreditCardAllPay(string merchantTradeNo, decimal amount, string tradeDesc)
    {
        var webAccess = new WebAccess
        {
            FormName = "PaymentByCc",
            Url = "http://payment-stage.allpay.com.tw/Cashier/AioCheckOut"//介接網址
        };

        webAccess.AddData("MerchantID", AllPayUtil.TestMerchantID);//特店編號(MerchantID)        
        webAccess.AddData("MerchantTradeNo", merchantTradeNo); // 廠商訂單編號      
        var tradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        webAccess.AddData("MerchantTradeDate", tradeDate);
        const string paymentType = "aio"; //請帶  aio
        webAccess.AddData("PaymentType", paymentType);
        var totalAmount = amount.ToString("0");
        webAccess.AddData("TotalAmount", totalAmount); // 訂單金額
        webAccess.AddData("TradeDesc", tradeDesc);//交易描述 
        webAccess.AddData("ItemName", tradeDesc);//商品名稱

        #region 付款完成通知回傳網址
        string env;

        if (Util.GetSslBaseUrl().StartsWith("https://test.host"))//測試機
        {
            env = "test";
        }
        else if (Util.GetSslBaseUrl().StartsWith("https://localhost") //本機
                 || Util.GetSslBaseUrl().StartsWith("https://192.168.")
                 || Util.GetSslBaseUrl().StartsWith("https://::1"))
        {
            env = "dev";
        }
        else
        {
            env = "prod";
        }

        //當消費者付款完成後，會將付款結果以 server端幕後方式， 回傳到該網址。
        string returnURL = string.Format(@"https://www.mydomain.tw/PaidPage?env={0}", env);// 訂單回傳網址(測試時也必須使用外部可連得到的網址)
        webAccess.AddData("ReturnURL", returnURL); 
        //此網址為付款完成後，銀行將頁面導回到歐付寶時， 歐付寶會再將頁面導回到此設定的網址，
        //並帶回付款結果的參數，沒帶此參數則會顯示歐付寶的顯示付款完成頁。
        //如果要將付款結果頁顯示在貴公司，請設定此網址。(有些銀行的 WebATM在交易成功後，
        //會停留在銀行的頁面，並不會導回給歐付寶，所以歐付寶也不會將頁面導回到 OrderResultURL 的頁面。 )
        string orderResultURL = string.Format(@"https://www.mydomain.tw/PaidResult?key={0}", merchantTradeNo);  
        webAccess.AddData("OrderResultURL", orderResultURL);//付款完成通知回傳網址
        #endregion

        //webAccess.AddData("NeedExtraPaidInfo","Y");//是否需要額外的付款資訊

        if (Util.IsFromMobile())//檢查是否是手機裝置
        {
            webAccess.AddData("DeviceSource", "M"); //  使用手機歐富寶網頁版本.
            webAccess.AddData("ChoosePayment", "ALL"); // 選擇預設付款方式
            webAccess.AddData("IgnorePayment", "CVS#WebATM#ATM#BARCODE#Alipay#Tenpay#TopUpUsed");//忽略的付款方式
        }
        else
        {
            webAccess.AddData("DeviceSource", "P"); // 使用非手機網頁版本.
            webAccess.AddData("ChoosePayment", "Credit"); // 選擇預設付款方式
        }



        #region　檢查碼

        string hashKey = AllPayUtil.TestHashKey;
        string hashIv = AllPayUtil.TestHashIv;
        var urlParameters = webAccess.GetUrlParameters();
        var checkMacValue = AllPayUtil.GetCheckMacValue(urlParameters, hashKey, hashIv);
        webAccess.AddData("CheckMacValue", checkMacValue); // 檢查碼   
        #endregion
        Logger.Log(string.Format("Order confirm {0}", merchantTradeNo));
        webAccess.SubmitAtClient();

    }


}