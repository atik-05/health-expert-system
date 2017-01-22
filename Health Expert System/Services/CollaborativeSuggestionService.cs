using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.Services
{
    public class CollaborativeSuggestionService
    {
        private SelectionRepository _selectionRepository = new SelectionRepository();
        private readonly DatabaseContext _databaseContext = new DatabaseContext();
        private List<CollaborationViewModel> _models;
        private List<SuggestionViewModel> _result = new List<SuggestionViewModel>();
        private int length;

        public CollaborativeSuggestionService(List<CollaborationViewModel> models)
        {
            this._models = models;
            length = _models.Count;
        }

        public List<SuggestionViewModel> GetServiceProviders()
        {
            StandardizedDistance();
            CalculateEuclideanDistance();
            SortEuclideanDistance();

            _result = CreateResultModel();


            
            return _result;
        }

        private void StandardizedDistance()
        {
            var temp = _models;

            temp = temp.OrderBy(s => s.Distance).ToList();
            var minOfDistance = temp[0].Distance;
            var maxOfDistance = temp[length - 1].Distance;

            temp = temp.OrderBy(s => s.Age).ToList();
            var minOfAge = temp[0].Age;
            var maxOfAge = temp[length - 1].Age;

            temp = temp.OrderBy(s => s.Income).ToList();
            var minOfIncome = temp[0].Income;
            var maxOfIncome = temp[length - 1].Income;

            for (int i = 0; i < length; i++)
            {
                var distanceValue = (temp[i].Distance - minOfDistance) / (maxOfDistance - minOfDistance);
                var ageValue = (temp[i].Age - minOfAge) / (maxOfAge - minOfAge);
                var incomeValue = (temp[i].Income - minOfIncome) / (maxOfIncome - minOfIncome);

                temp[i].StandardDistance = distanceValue;
                temp[i].Age = ageValue;
                temp[i].Income = incomeValue;
            }
            _models = temp;
        }

        private void CalculateEuclideanDistance()
        {
            var length = _models.Count;

            var currentUserId = WebSecurity.CurrentUserId;
            var user = _databaseContext.PatientProfiles.FirstOrDefault(s=>s.UserId == currentUserId);
            var userAge = user.Age;
            var userIncome = user.MonthlyIncome;
            for (int i = 0; i < length; i++)
            {
                var differenceOfAge = _models[i].Age - userAge;
                var differenceOfIncome = _models[i].Income - userIncome;

                float distance = (float) Math.Sqrt(_models[i].StandardDistance*_models[i].StandardDistance + differenceOfIncome*differenceOfIncome + differenceOfAge*differenceOfAge);
                _models[i].EuclideanDistance = distance;
            }
        }

        private void SortEuclideanDistance()
        {
            _models = _models.OrderBy(s => s.EuclideanDistance).ToList();
        }

        private List<SuggestionViewModel> CreateResultModel()
        {
            for (int i = 0; i < length; i++)
            {
                var providerId = _models[i].ServiceProviderId;
                if (i>0)
                {
                    if (_models[i-1].ServiceProviderId == _models[i].ServiceProviderId)
                    {
                        _models.Remove(_models[i]);
                        continue;
                    }
                }
                var result = _databaseContext.ServiceProviders.Find(providerId);

                var finalResult = new SuggestionViewModel
                {
                    ServiceProviderId = providerId,
                    Type = result.Type,
                    Name = result.Name,
                    Address = result.Address,
                    Rating = result.Rating,
                    Distance = _models[i].Distance,
                };
                if (result.Type=="Doctor")
                {
                    finalResult.Fee =
                        _databaseContext.DoctorProfiles.FirstOrDefault(s => s.ServiceProviderId == providerId)
                            .VisitingFee;
                    finalResult.Specialist = (from doctor in _databaseContext.DoctorProfiles
                        join specialist in _databaseContext.Specialistses on doctor.SpecialistId equals specialist.Id
                        select specialist).FirstOrDefault().Name;

                }
                else
                {
                    finalResult.Fee =
                        _databaseContext.HospitalProfiles.FirstOrDefault(s => s.ServiceProviderId == providerId)
                            .CabinFee;
                }
                _result.Add(finalResult);

                if (_result.Count == 2)
                {
                    break;
                }

            }
            return _result.ToList();
        }
    }
}