using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    [InitializeSimpleMembership]
    public class RegistrationController : Controller
    {
        private PatientService _patientService = new PatientService();
        private DoctorService _doctorService = new DoctorService();
        private HospitalService _hospitalService = new HospitalService();
        private ServiceProviderSevice _serviceProviderSevice = new ServiceProviderSevice();
        private readonly PatientRepository _repository = new PatientRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterPatient()
        {
            return View();
        }

        [HttpPost]
        public void RegisterPatient(PatientRegistrationViewModel patient)
        {
            _patientService.AddPatient(patient);
        }

        [HttpGet]
        public ActionResult RegisterDoctor()
        {
            return View();
        }

        [HttpPost]
        public void RegisterDoctor(ServiceProviderViewModel doctor)
        {
            _doctorService.AddDoctor(doctor);
        }

        [HttpGet]
        public ActionResult RegisterHospital()
        {
            return View();
        }
                                    
        [HttpPost]
        public void RegisterHospital(ServiceProviderViewModel hospital)
        {
            _hospitalService.AddHospital(hospital);
        }

        public bool IsExistUser(string userName, int registrationNo)
        {
            bool isExist = _repository.IsExistUser(userName, registrationNo);
            return isExist;
        }
        public bool IsExistHospital(string userName)
        {
            bool isExist = _repository.IsExistHospital(userName);
            return isExist;
        }

        [HttpGet]
        public ActionResult TestPatient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestPatient(LoginModel model)
        {

            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
            var id = WebSecurity.Login(model.UserName, model.Password);
            var pid = WebSecurity.GetUserId(model.UserName);
            var patient = WebSecurity.CurrentUserName;
            return View();
        }

        public JsonResult GetSpecialists()
        {
            var sp = _serviceProviderSevice.GetSpecialists();
            return Json(sp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestMap()
        {
            return View();
        }

        public JsonResult Check(int id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bmdc.org.bd/doctors-info/");
            request.Method = "POST";

            var formContent = "search_doc_id=" + id + "&type=1&action=Search_doctors";

            byte[] byteArray = Encoding.UTF8.GetBytes(formContent);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = HttpUtility.UrlDecode(reader.ReadToEnd());
            //You may need HttpUtility.HtmlDecode depending on the response

            reader.Close();
            dataStream.Close();
            response.Close();

            bool isExist = false;

            if (responseFromServer.Contains("<td>Doctor's Name</td>"))
            {
                //responseFromServer = responseFromServer.Remove(0, responseFromServer.IndexOf("<td>Doctor's Name</td>", StringComparison.Ordinal));
                //responseFromServer = responseFromServer.Remove(0, responseFromServer.IndexOf("<td>: </td>", StringComparison.Ordinal) + 5);
                //responseFromServer = responseFromServer.Remove(0, responseFromServer.IndexOf("<td>", StringComparison.Ordinal) + 4);
                //responseFromServer = responseFromServer.Remove(responseFromServer.IndexOf("</td>", StringComparison.Ordinal));
                isExist = true;
            }
            else if (responseFromServer.Contains("Not found data for your Search."))
            {
                //responseFromServer = "not found";
                isExist = false;
            }
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}