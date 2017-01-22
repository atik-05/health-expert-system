using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Health_Expert_System.DAL;
using Health_Expert_System.Fileters;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using Health_Expert_System.Services;
using WebMatrix.WebData;

namespace Health_Expert_System.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class SuggestionController : Controller
    {
        private readonly ServiceProviderSevice _providerSevice = new ServiceProviderSevice();
        private readonly DiseaseRepository _diseaseRepository = new DiseaseRepository();
        private SuggestionService _suggestionService = new SuggestionService();
        private readonly SelectionRepository _selectionRepository = new SelectionRepository();

        // GET: Suggestion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SuggestServiceProvider()
        {
            return View();
        }

        public JsonResult GetPatientData(RequiredDataForSuggestion requiredData)
        {
               var result = _suggestionService.SuggestServiceProvider(requiredData);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceProviders()
        {
            return Json(_providerSevice.GetServiceProviders(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSpecialists()
        {
            return Json(_diseaseRepository.GetSpecialists(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSymptoms()
        {
            return Json(_diseaseRepository.GetSymptoms(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDerivedSymptoms()
        {
            return Json(_diseaseRepository.GetDerivedSymptoms(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCurrentUserChild()
        {
            return Json(_diseaseRepository.IsCurrentUserChild(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequiredHospital(int id)
        {
            HospitalRepository hos = new HospitalRepository();
            return Json(hos.GetRequiredHospital(id), JsonRequestBehavior.AllowGet);
        }

        public List<SelectionHistory> GetSelectionHistory()
        {
            var history = _selectionRepository.GetSelectionHistories();
            return history;
        }

        
        
    }
}