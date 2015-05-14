using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoPageData : BasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PageDataDic.Add("no", 9);
        PageDataDic.Add("name", "This is CmDemo Page");
        PageDataDic.Add("data", new 
        {
            Name= "Marsen Lin",
            Products = getDemoData()
        });
        hidPageData.Value = DataConverter.Serialize(PageDataDic);
    }

    private List<DemoProdcuts> getDemoData()
    {
        var result = new List<DemoProdcuts>
        {
            new DemoProdcuts
            {
                No = "ML-309",
                Length = 170,
                Weight = 67,
                Guid = new Guid()
            },
            new DemoProdcuts
            {
                No = "LE-256",
                Length = 164,
                Weight = 60,
                Guid = new Guid()
            },
            new DemoProdcuts
            {
                No = "TEST-P",
                Length = 10,
                Weight = 100,
                Guid = new Guid()
            }
        };
        return result;
    }

    private class DemoProdcuts
    {
        public string No { get; set; }    
        public uint Length { get; set; }
        public decimal Weight { get; set; }
        public Guid Guid { get; set; }
    }
}