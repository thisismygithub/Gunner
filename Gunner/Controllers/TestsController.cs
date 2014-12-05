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

        public ActionResult Css()
        {
            return View();
        }
    }
}