using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;
using Health_Expert_System.Models.EnumClass;
using WebMatrix.WebData;

namespace Health_Expert_System.DAL
{
    public class DiseaseRepository
    {
        DatabaseContext db = new DatabaseContext();

        public List<Specialists> GetSpecialists()
        {
            return db.Specialistses.ToList();
        }

        public List<Symptoms> GetSymptoms()
        {
            int patientType;
            var id = WebSecurity.CurrentUserId;
            var patient = db.PatientProfiles.FirstOrDefault(s=> s.UserId == id);

            var gender = patient.Gender;
            if (gender == "M")
            {
                patientType = (int)PatientTypeEnum.Male;
            }
            else
            {
                patientType = (int)PatientTypeEnum.Female;
            }

            var symptoms = from sym in db.Symptomses
                           where sym.PatientType == patientType || sym.PatientType == (int)PatientTypeEnum.Both
                           select sym;

            return symptoms.ToList();

        }
        public List<DerivedSymptoms> GetDerivedSymptoms()
        {
            return db.DerivedSymptomses.ToList();
        }

        public bool IsCurrentUserChild()
        {
            var id = WebSecurity.CurrentUserId;
            var patient = db.PatientProfiles.FirstOrDefault(s => s.UserId==id);
            if (patient.Age <12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}