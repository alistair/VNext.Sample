using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AWStruck.Hubs;

namespace AWStruck.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var booboo = new HublyBubly();

            booboo.Hello("world");

            return View();
        }
    }
}
