using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;

namespace Health_Expert_System.Services
{
    public class ServiceProviderSevice
    {
        private readonly ServiceProviderRepository _repository = new ServiceProviderRepository();
        private readonly DiseaseRepository _diseaseRepository = new DiseaseRepository();
        private readonly DoctorRepository _doctorRepository = new DoctorRepository();
        private readonly HospitalRepository _hospitalRepository = new HospitalRepository();
        private DatabaseContext _databaseContext = new DatabaseContext();
        private List<SuggestionViewModel> processedData = new List<SuggestionViewModel>();

        public List<ServiceProvider> GetServiceProviders()
        {
            return  _repository.GetServiceProviders();
        }

        public List<Specialists> GetSpecialists()
        {
            return _diseaseRepository.GetSpecialists();
        }

        public List<SuggestionViewModel> GetProvidersData(List<AllDistance> allDistances)
        {
            processedData = PreprocessOfData(allDistances);
            processedData = processedData.OrderBy(s => s.Distance).ToList();

            return processedData;
        }

        private List<SuggestionViewModel> PreprocessOfData(List<AllDistance> allDistances)
        {
            var serviceProviders = _repository.GetServiceProviders();
            var doctors = _doctorRepository.GetDoctorProfiles();
            var hospitals = _hospitalRepository.GetHospitalProfiles();

            var analysisData1 = from doctor in doctors
                                join specialist in _databaseContext.Specialistses on doctor.SpecialistId equals specialist.Id
                                select new SuggestionViewModel
                                {
                                    ServiceProviderId = doctor.ServiceProviderId,
                                    Specialist = specialist.Name,
                                    Fee = doctor.VisitingFee,
                                    IsGovt = false
                                };
            var analysisData2 = from hospital in hospitals
                                select new SuggestionViewModel
                                {
                                    ServiceProviderId = hospital.ServiceProviderId,
                                    Specialist = "general",
                                    Fee = hospital.CabinFee,
                                    IsGovt = hospital.IsGovt
                                };
            var analysisData = analysisData1.Concat(analysisData2);

            var result = from data in analysisData
                         join providerData in allDistances on data.ServiceProviderId equals providerData.Id
                         join serviceProvider in _databaseContext.ServiceProviders on providerData.Id equals serviceProvider.Id
                         select new SuggestionViewModel
                         {
                             ServiceProviderId = data.ServiceProviderId,
                             Name = serviceProvider.Name,
                             Specialist = data.Specialist,
                             Address = serviceProvider.Address,
                             Fee = data.Fee,
                             Rating = serviceProvider.Rating,
                             Distance = providerData.Distance,
                             IsGovt = data.IsGovt
                         };
            return result.ToList();
        }
    }
}