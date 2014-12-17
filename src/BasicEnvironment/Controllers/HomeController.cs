using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;

namespace BasicEnvironment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            SetEnvironmentName();
            return View();
        }

        private void SetEnvironmentName()
        {
            var configuration = new Configuration()
                     .AddJsonFile("settings.json")
                     .AddEnvironmentVariables();

            if (configuration.Get("ENV") != null)
            {
                var envSpecificJson = configuration.Get("ENV") + "_settings.json";
                configuration.AddJsonFile(envSpecificJson);
            }

            var envName = configuration.Get("environmentName");
            ViewBag.Name = envName;
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}