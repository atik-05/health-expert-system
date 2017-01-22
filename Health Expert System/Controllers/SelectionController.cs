using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Health_Expert_System.DAL;
using Health_Expert_System.Fileters;
using Health_Expert_System.Models;
using WebMatrix.WebData;

namespace Health_Expert_System.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class SelectionController : Controller
    {
        private readonly SelectionRepository _selectionRepository = new SelectionRepository();
        // GET: Selection
        public ActionResult Index()
        {
            return View();
        }

        public void SelectServiceProvider(int id, int diseaseId, float latitude, float longitude)
        {
            //for child
            if (diseaseId==0)
            {
                diseaseId = 23;
            }
            _selectionRepository.SaveSelection(id, diseaseId, latitude, longitude);
        }

        public JsonResult GetSelectionHistory()
        {
            var history = _selectionRepository.GetSelectionHistories();
            return Json(history, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotRatedProvider()
        {
            int userId = WebSecurity.CurrentUserId;
            SelectionRepository selection = new SelectionRepository(userId);
            return Json(selection.GetNotRatedProvider(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRecentTreatmentHistory()
        {
            int userId = WebSecurity.CurrentUserId;
            SelectionRepository selection = new SelectionRepository(userId);
            return Json(selection.GetRecentTreatmentHistory(), JsonRequestBehavior.AllowGet);
        }
    }
}