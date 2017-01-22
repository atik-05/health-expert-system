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
    public class HospitalService
    {
        private readonly ServiceProviderRepository _providerRepository = new ServiceProviderRepository();
        private readonly HospitalRepository _hospitalRepository = new HospitalRepository();

        public void AddHospital(ServiceProviderViewModel serviceProvider)
        {
            WebSecurity.CreateUserAndAccount(serviceProvider.UserName, serviceProvider.Password);
            WebSecurity.Login(serviceProvider.UserName, serviceProvider.Password);
            Roles.AddUserToRole(serviceProvider.UserName, "Hospital");

            var userId = WebSecurity.GetUserId(serviceProvider.UserName);


            var service = new ServiceProvider
            {
                UserId = userId,
                Name = serviceProvider.Name,
                Address = serviceProvider.Address,
                Type = serviceProvider.Type,
                Latitude = serviceProvider.Latitude,
                Longitude = serviceProvider.Longitude,
                Rating = serviceProvider.Rating
            };

            int serviceId = _providerRepository.AddServiceProvider(service);

            var hospital = new HospitalProfile
            {
                ServiceProviderId = serviceId,
                HospitalTypeId = serviceProvider.HospitalTypeId,
                ListOfSpecialistId = serviceProvider.ListOfSpecialistId,
                CabinFee = serviceProvider.CabinFee,
                IsGovt = serviceProvider.IsGovt
            };

            _hospitalRepository.AddHospitalProfile(hospital);

        }
    }
}