using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;

namespace Health_Expert_System.DAL
{
    public class PatientRepository
    {
        readonly DatabaseContext _databaseContext = new DatabaseContext();

        public List<PatientProfile> GetPatientProfiles()
        {
            return _databaseContext.PatientProfiles.ToList();
        }

        public void AddPatientProfile(PatientProfile patient)
        {
            _databaseContext.PatientProfiles.Add(patient);
            _databaseContext.SaveChanges();
        }

        public bool IsExistUser(string userName, int registrationNo)
        {
            var user = _databaseContext.UserProfiles.FirstOrDefault(s => s.UserName.ToLower() == userName.ToLower());
            var duplicateRegister =
                _databaseContext.DoctorProfiles.FirstOrDefault(s => s.RegistrationNumber == registrationNo);
            if (user != null || duplicateRegister!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsExistHospital(string userName)
        {
            var user = _databaseContext.UserProfiles.FirstOrDefault(s => s.UserName.ToLower() == userName.ToLower());
            
            if (user != null)
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