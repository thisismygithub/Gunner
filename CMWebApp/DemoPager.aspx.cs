using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoPager : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int page;
        int.TryParse(Request.QueryString["page"],out page);
        if (page < 1)
        {
            page = 1;
        }
        PageDataDic.Add("page", page);
        hidPageData.Value = DataConverter.Serialize(PageDataDic);
    }
}