using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gunner.Models;

namespace Gunner.Controllers
{
    public class ArsenalController : Controller
    {
        private ArsenalDbContext db = new ArsenalDbContext();
        // GET: Arsenal
        public ActionResult Index()
        {
            var fixtures = new List<Fixtures>();
            //fixtures.Add(new Fixtures());
            return View();
        }
    }
}