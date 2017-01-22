using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Health_Expert_System.Models;

namespace Health_Expert_System.Controllers
{
    public class LocationController : Controller
    {

        // GET: Location
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowPresentLocation()
        {
            return View();
        }

        public ActionResult MyLocation()
        {
            return View();
        }

        public ActionResult SetInformation()
        {
            return View();
        }

        //driving distance
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ShowNearLocation()
        {
            return View();
        }

        public ActionResult ClickOnAddress()
        {
            return View();
        }

        public ActionResult TestDropdown()
        {
            return View();
        }

        public ActionResult FinalTestDropdown()
        {
            return View();
        }

        


    }
}