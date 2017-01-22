using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Health_Expert_System.Fileters;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using Health_Expert_System.Services;
using WebMatrix.WebData;

namespace Health_Expert_System.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private ServiceProviderSevice _providerSevice = new ServiceProviderSevice();
        private HomeService _homeService = new HomeService();
        private RatingService _ratingService;
        

        public ActionResult Index()
        {
            return View();
        }

        
        public void StoreRating(Comment comment)
        {
            comment.PatientId = WebSecurity.CurrentUserId;
            _ratingService = new RatingService(comment);
            _ratingService.StoreRating();
        }

        public ActionResult Search()
        {
            return View();
        }

        public JsonResult GetProvidersData(List<AllDistance> allDistances)
        {
            var result = _providerSevice.GetProvidersData(allDistances);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public JsonResult GetUserProfile()
        {
            var userId = WebSecurity.CurrentUserId;
            var userProfile = _homeService.GetUserProfile(userId);
            return Json(userProfile, JsonRequestBehavior.AllowGet);
        }

        public void SaveProfile(UserProfileViewModel user)
        {
            var userId = WebSecurity.CurrentUserId;
            _homeService.SaveProfile(user, userId);
        }
        
    }
}