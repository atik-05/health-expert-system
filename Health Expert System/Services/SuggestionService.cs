using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;

namespace Health_Expert_System.Services
{
    public class SuggestionService
    {
        private DoctorRepository _doctorRepository = new DoctorRepository();
        private HospitalRepository _hospitalRepository = new HospitalRepository();
        private DatabaseContext _databaseContext = new DatabaseContext();
        private readonly int ChildSpecialistId = 10;
        private EmergencyService _emergency;
        private NotEmergencyService _notEmergency;
        private CollaborativeSuggestionService _collaborativeSuggestion;
        private List<SuggestionViewModel> processedData = new List<SuggestionViewModel>();

        public List<SuggestionViewModel> SuggestServiceProvider(RequiredDataForSuggestion requiredData)
        {
            if (requiredData.IsEmergency)
            {
                processedData = PreprocessOfData(requiredData);
                _emergency = new EmergencyService(processedData);
                processedData = _emergency.GetEmergencyServiceProvider();
            }
            else
            {
                if (requiredData.SuggestionTypeId == 1)
                {
                    processedData = PreprocessOfData(requiredData);
                    _notEmergency = new NotEmergencyService(processedData);
                    processedData = _notEmergency.GetServiceProvider();

                }

                if (requiredData.SuggestionTypeId == 2)
                {
                    var data = CollaborativeProcessOfData(requiredData);
                    _collaborativeSuggestion = new CollaborativeSuggestionService(data);
                    processedData = _collaborativeSuggestion.GetServiceProviders();
                }
            }
            

            return processedData;

        }

        public int GetSpecialistId(int symptomId, int derivedSymptomId)
        {
            int specialistId = 0;
            if (symptomId == 0 && derivedSymptomId == 0)
            {
                specialistId = ChildSpecialistId;
            }
            else if (derivedSymptomId == 0)
            {
                var symptom = _databaseContext.Symptomses.Find(symptomId);
                specialistId = symptom.SpecialistId;
            }
            else
            {
                var derivedSymptom = _databaseContext.DerivedSymptomses.Find(derivedSymptomId);
                specialistId = derivedSymptom.SpecialistId;
            }

            return specialistId;
        }

        private List<SuggestionViewModel> PreprocessOfData(RequiredDataForSuggestion requiredData)
        {
            int specialistId = GetSpecialistId(requiredData.SymptomId, requiredData.DerivedSymptomId);
            var specialistName = _databaseContext.Specialistses.Find(specialistId).Name;
            var doctors = _doctorRepository.GetRequiredDoctor(specialistId);
            var hospitals = _hospitalRepository.GetRequiredHospital(specialistId);
            var analysisData1 = from doctor in doctors
                                select new SuggestionViewModel
                                {
                                    ServiceProviderId = doctor.ServiceProviderId,
                                    Type = "Doctor",
                                    Fee = doctor.VisitingFee,
                                    IsGovt = false
                                };
            var analysisData2 = from hospital in hospitals
                                select new SuggestionViewModel
                                {
                                    ServiceProviderId = hospital.ServiceProviderId,
                                    Type = "Hospital",
                                    Fee = hospital.CabinFee,
                                    IsGovt = hospital.IsGovt
                                };
            var analysisData = analysisData1.Concat(analysisData2);

            var result = from data in analysisData
                         join providerData in requiredData.AllDistances on data.ServiceProviderId equals providerData.Id
                         join serviceProvider in _databaseContext.ServiceProviders on providerData.Id equals serviceProvider.Id
                         select new SuggestionViewModel
                         {
                             ServiceProviderId = data.ServiceProviderId,
                             Name = serviceProvider.Name,
                             Type = data.Type,
                             Specialist = specialistName,
                             Address = serviceProvider.Address,
                             Fee = data.Fee,
                             Rating = serviceProvider.Rating,
                             Distance = providerData.Distance,
                             IsGovt = data.IsGovt
                         };
            return result.ToList();
        }

        private List<CollaborationViewModel> CollaborativeProcessOfData(RequiredDataForSuggestion requiredData)
        {
            int specialistId = GetSpecialistId(requiredData.SymptomId, requiredData.DerivedSymptomId);
            var doctors = _doctorRepository.GetRequiredDoctor(specialistId);
            var hospitals = _hospitalRepository.GetRequiredHospital(specialistId);
            var analysisData1 = from doctor in doctors
                                select new CollaborationViewModel
                                {
                                    ServiceProviderId = doctor.ServiceProviderId,
                                    Type = "Doctor"
                                };
            var analysisData2 = from hospital in hospitals
                                select new CollaborationViewModel
                                {
                                    ServiceProviderId = hospital.ServiceProviderId,
                                    Type = "Hospital"
                                };
            var analysisIds = analysisData1.Concat(analysisData2);
            var patients = _databaseContext.PatientProfiles;

            var selectedHistroy = from requiredProvider in analysisIds
                                  join history in _databaseContext.SelectionHistories on requiredProvider.ServiceProviderId equals history.ServiceProviderId
                                  join distance in requiredData.AllDistances on history.Id equals distance.Id
                                  from patient in _databaseContext.PatientProfiles
                                  where history.PatientId == patient.UserId
                                  select new CollaborationViewModel
                                  {
                                      HistoryId = history.Id,
                                      Distance = distance.Distance,
                                      PatientId = history.PatientId,
                                      Age = patient.Age,
                                      Income = patient.MonthlyIncome,
                                      ServiceProviderId = requiredProvider.ServiceProviderId,
                                      Type = requiredProvider.Type
                                  };

            return selectedHistroy.ToList();
        }
    }
}