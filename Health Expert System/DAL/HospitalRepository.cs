using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;

namespace Health_Expert_System.DAL
{
    public class HospitalRepository
    {

        readonly DatabaseContext _databaseContext = new DatabaseContext();

        public List<HospitalProfile> GetHospitalProfiles()
        {
            return _databaseContext.HospitalProfiles.ToList();
        }

        public void AddHospitalProfile(HospitalProfile hospital)
        {
            _databaseContext.HospitalProfiles.Add(hospital);
            _databaseContext.SaveChanges();
        }

        public List<HospitalProfile> GetRequiredHospital(int specialistId)
        {
            List<HospitalProfile> list = new List<HospitalProfile>();
            foreach (var hospital in _databaseContext.HospitalProfiles)
            {
                if (hospital.HospitalTypeId==1)
                {
                    list.Add(hospital);
                }
                else 
                {
                    int[] ids = hospital.ListOfSpecialistId.Split(',').Select(int.Parse).ToArray();
                    var isHave = ids.Contains(specialistId);
                    if (isHave)
                    {
                        list.Add(hospital);
                    }
                }
                
                
            }
            //int[] hos = _databaseContext.HospitalProfilesSplit(',').Select(s => int.Parse(s)).ToArray();
            //var sp = specialistId.ToString();
            //var hospital = (from hos in _databaseContext.HospitalProfiles
            //               where hos.ListOfSpecialistId.Contains(sp)
            //               select hos).ToList();


            //var rows = _databaseContext.HospitalProfiles.Where(r => r.ListOfSpecialistId.Split(Convert.ToChar(",")).Any(s => s == specialistId));
            //var h = _databaseContext.HospitalProfiles.Where(r => r.ListOfSpecialistId.Split(',').Any(s=> s==sp)).ToList();
            //var hospitals = _databaseContext.HospitalProfiles.Contains(sp);
            //return doctors;
            return list;
        }
    }
}