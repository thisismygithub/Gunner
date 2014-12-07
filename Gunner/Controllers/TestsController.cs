using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gunner.Backend;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gunner.Controllers
{
    public class TestsController : Controller
    {
        // GET: Tests
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Json()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Json(string json)
        {
            JObject jo =
            JsonConvert.DeserializeObject<JObject>(json);
            JsonClassParser.RegisterClass(jo, "Root");
            var cls = JsonClassParser.GenClassCode();
            var model = new
            {
                jsonKey = cls
            };
            ViewBag.jsonKey= cls;
            return View(model);
            
        }

        public ActionResult Css(string id)
        {
            ViewResult result;
            var action = id ?? string.Empty;
            //TODO:Marsen  without string switch
            switch (action.ToLower())
            {
                case "onecolume":
                    result = View("~/Views/Tests/Css/OneColume.cshtml");
                    break;
                case "onecolumehtml5":
                    result = View("~/Views/Tests/Css/OneColumeHTML5.cshtml");
                    break;
                case "twocolume":
                    result = View("~/Views/Tests/Css/TwoColume.cshtml");
                    break;
                case "twocolumehtml5":
                    result = View("~/Views/Tests/Css/TwoColume.cshtml");
                    break;
                case "threecolume":
                    result = View("~/Views/Tests/Css/ThreeColume.cshtml");
                    break;
                case "threecolumehtml5":
                    result = View("~/Views/Tests/Css/ThreeColume.cshtml");
                    break;
                default:
                    result =  View("~/Views/Tests/Css/Css.cshtml");
                    break;
            }
            return result;
        }
    }
}