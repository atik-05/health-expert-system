using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;

namespace Health_Expert_System.Services
{
    public class HomeService
    {
        private DatabaseContext _databaseContext = new DatabaseContext();

        public UserProfileViewModel GetUserProfile(int userId)
        {
            UserProfileViewModel model = new UserProfileViewModel();
            if (Roles.IsUserInRole("Patient"))
            {
                var patient = _databaseContext.PatientProfiles.FirstOrDefault(s => s.UserId == userId);
                model = new UserProfileViewModel
                {
                    Name = patient.Name,
                    Age = patient.Age,
                    Occupation = patient.Occupation,
                    MonthlyIncome = patient.MonthlyIncome,
                    LifestyleStatus = patient.LifestyleStatus,
                    Type = "Patient"
                };
            }
            else if (Roles.IsUserInRole("Doctor"))
            {
                model = (from serviceProvider in _databaseContext.ServiceProviders
                    where serviceProvider.UserId == userId
                    join doctorProfile in _databaseContext.DoctorProfiles on serviceProvider.Id equals
                        doctorProfile.ServiceProviderId
                    join specialist in _databaseContext.Specialistses on doctorProfile.SpecialistId equals specialist.Id
                    select new UserProfileViewModel
                    {
                        Name = serviceProvider.Name,
                        Specialist = specialist.Name,
                        YearOfExperience = doctorProfile.YearOfExperience,
                        VisitingFee = doctorProfile.VisitingFee,
                        Rating = serviceProvider.Rating,
                        Type = "Doctor"
                    }).FirstOrDefault();

            }
            else if (Roles.IsUserInRole("Hospital"))
            {
                model = (from serviceProvider in _databaseContext.ServiceProviders
                         where serviceProvider.UserId == userId
                         join hospitalProfile in _databaseContext.HospitalProfiles on serviceProvider.Id equals
                             hospitalProfile.ServiceProviderId
                         select new UserProfileViewModel
                         {
                             Name = serviceProvider.Name,
                             CabinFee = hospitalProfile.CabinFee,
                             Rating = serviceProvider.Rating,
                             Type = "Hospital"
                         }).FirstOrDefault();
            }
            return model;
        }

        public void SaveProfile(UserProfileViewModel user, int userId)
        {
            if (Roles.IsUserInRole("Patient"))
            {
                var patient = _databaseContext.PatientProfiles.FirstOrDefault(s => s.UserId == userId);
                patient.Name = user.Name;
                patient.Age = user.Age;
                patient.Occupation = user.Occupation;
                patient.MonthlyIncome = user.MonthlyIncome;
                patient.LifestyleStatus = user.LifestyleStatus;

                _databaseContext.SaveChanges();
            }
            else if (Roles.IsUserInRole("Doctor"))
            {
                var serviceProvider = _databaseContext.ServiceProviders.FirstOrDefault(s => s.UserId == userId);
                serviceProvider.Name = user.Name;

                var doctor =
                    _databaseContext.DoctorProfiles.FirstOrDefault(s => s.ServiceProviderId == serviceProvider.Id);
                doctor.YearOfExperience = user.YearOfExperience;
                doctor.VisitingFee = user.VisitingFee;

                _databaseContext.SaveChanges();
            }
            else if (Roles.IsUserInRole("Hospital"))
            {
                var serviceProvider = _databaseContext.ServiceProviders.FirstOrDefault(s => s.UserId == userId);
                serviceProvider.Name = user.Name;

                var hospital =
                    _databaseContext.HospitalProfiles.FirstOrDefault(s => s.ServiceProviderId == serviceProvider.Id);
                hospital.CabinFee = user.CabinFee;


                _databaseContext.SaveChanges();
            }
        }
    }
}