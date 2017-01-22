using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Health_Expert_System.Models;

namespace Health_Expert_System.DAL
{
    public class DoctorRepository
    {
        readonly DatabaseContext _databaseContext = new DatabaseContext();

        public List<DoctorProfile> GetDoctorProfiles()
        {
            return _databaseContext.DoctorProfiles.ToList();
        }

        public void AddDoctorProfile(DoctorProfile doctor)
        {
            _databaseContext.DoctorProfiles.Add(doctor);
            _databaseContext.SaveChanges();
        }

        public List<DoctorProfile> GetRequiredDoctor(int specialistId)
        {
            var doctors = _databaseContext.DoctorProfiles.Where(s => s.SpecialistId == specialistId).ToList();
            return doctors;
        }
    }
}