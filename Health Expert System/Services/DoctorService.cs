using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.Services
{
    public class DoctorService
    {
        private readonly ServiceProviderRepository _providerRepository = new ServiceProviderRepository();
        private readonly DoctorRepository _doctorRepository = new DoctorRepository();

        public void AddDoctor(ServiceProviderViewModel serviceProvider)
        {
            WebSecurity.CreateUserAndAccount(serviceProvider.UserName, serviceProvider.Password);
            WebSecurity.Login(serviceProvider.UserName, serviceProvider.Password);
            Roles.AddUserToRole(serviceProvider.UserName, "Doctor");

            var userId = WebSecurity.GetUserId(serviceProvider.UserName);

            var service = new ServiceProvider
            {
                UserId = userId,
                Name = serviceProvider.Name,
                Type = serviceProvider.Type,
                Address = serviceProvider.Address,
                Latitude = serviceProvider.Latitude,
                Longitude = serviceProvider.Longitude,
                Rating = serviceProvider.Rating
            };

            int serviceId = _providerRepository.AddServiceProvider(service);

            var doctor = new DoctorProfile
            {
                ServiceProviderId = serviceId,
                RegistrationNumber = serviceProvider.RegistrationNumber,
                SpecialistId = serviceProvider.SpecialistId,
                YearOfExperience = serviceProvider.YearOfExperience,
                VisitingFee = serviceProvider.VisitingFee,
                StartAndEndTime = serviceProvider.StartAndEndTime
            };
            
            _doctorRepository.AddDoctorProfile(doctor);

        }
    }
}