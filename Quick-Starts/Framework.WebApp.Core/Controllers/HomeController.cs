using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Framework.WebApp.Models;

namespace Framework.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public const string ControllerName = "Home";
        public const string ContactUsView = "~/Views/Genesys-Source/Genesys-Contact.cshtml";
        public const string IndexGetView = "~/Views/Home/Index.cshtml";
        public const string IndexGetAction = "Index";
        public const string IndexPostAction = "IndexPost";
        public const string IndexPutAction = "IndexPut";
        public const string IndexDeleteAction = "IndexDelete";

        /// <summary>
        /// Default HttpGet route
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpPost route
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult IndexPost()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpPut route
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public ActionResult IndexPut()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpDelete route
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public ActionResult IndexDelete()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Products view
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult CompareProducts()
        {
            return View();
        }

        /// <summary>
        /// Compare view
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult CompareStack()
        {
            return View();
        }

        /// <summary>
        /// Compare view
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult CompareInfrastructure()
        {
            return View();
        }

        /// <summary>
        /// Error Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
           return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
