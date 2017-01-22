using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.Services
{
    public class NotEmergencyService
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();
        private List<SuggestionViewModel> _model;
        private List<SuggestionViewModel> _tempModel;
        private List<SuggestionViewModel> resultModel = new List<SuggestionViewModel>();
        private string _status;
        private readonly int _scope = 10;

        public NotEmergencyService(List<SuggestionViewModel> model)
        {
            this._model = model;
            SetPatient_status();
        }

        public List<SuggestionViewModel> GetServiceProvider()
        {
            SortModelByDistance();

            if (_status == "poor")
            {
                var provider = GetNearestGovtHospital();
                resultModel.Add(provider);
                _model.Remove(provider);
                resultModel.Add(GetSecondServiceProviderForPoor());
            }
            else if (_status == "moderate")
            {
                var provider = GetFirstServiceProviderForModerateOrRich();
                resultModel.Add(provider);
                _model.Remove(provider);
                resultModel.Add(GetSecondServiceProviderForModerate());
            }
            else if (_status == "high")
            {
                var provider = GetFirstServiceProviderForModerateOrRich();
                resultModel.Add(provider);
                _model.Remove(provider);
                resultModel.Add(GetSecondServiceProviderForRich());
            }
            


            return resultModel;
        }

        private SuggestionViewModel GetNearestGovtHospital()
        {
            _tempModel = _model;
            var govtHospital = _tempModel.FirstOrDefault(s => s.IsGovt == true);
            return govtHospital;
        }

        private void SortModelByDistance()
        {
            _model = _model.OrderBy(m => m.Distance).ToList();
        }

        private void SetPatient_status()
        {
            int userId = WebSecurity.CurrentUserId;
            var patient = _dbContext.PatientProfiles.FirstOrDefault(s => s.UserId == userId);
            if (patient != null)
                _status = patient.LifestyleStatus;
        }

        private SuggestionViewModel GetSecondServiceProviderForPoor()
        {
            _tempModel = _model;
            var length = _tempModel.Count;
            var itemToRemove = length - _scope;
            if (length > 10)
            {
                _tempModel.RemoveRange(_scope, itemToRemove);
            }
            _tempModel =
                _tempModel.OrderBy(s => s.Fee)
                .ThenBy(s => s.Distance)
                .ThenByDescending(s => s.Rating)
                .ToList();
            return _tempModel[0];

        }

        private SuggestionViewModel GetFirstServiceProviderForModerateOrRich()
        {
            _tempModel = _model;
            var length = _tempModel.Count;
            var itemToRemove = length - _scope;
            if (length > 10)
            {
                _tempModel.RemoveRange(_scope, itemToRemove);
            }
            _tempModel =
                _tempModel.OrderByDescending(s => s.Rating)
                    .ThenBy(s => s.Fee)
                    .ThenBy(s => s.Distance).ToList();
            return _tempModel[0];
        }

        private SuggestionViewModel GetSecondServiceProviderForModerate()
        {
            _tempModel = _model;
            _tempModel = _tempModel.OrderBy(s => s.Fee).ToList();
            var length = _tempModel.Count;
            var midle = length/3;
            if (length > 6)
            {
                _tempModel.RemoveRange(0, midle);
                _tempModel.RemoveRange(midle, midle);
            }
            _tempModel = _tempModel.OrderByDescending(s => s.Rating)
                .ThenBy(s => s.Distance).ToList();
            return _tempModel[0];
        }

        private SuggestionViewModel GetSecondServiceProviderForRich()
        {
            _tempModel = _model;
            _tempModel = _tempModel.OrderByDescending(s=>s.Fee).ToList();
            var length = _tempModel.Count;
            var itemToRemove = length - 5;
            if (length > 5)
            {
                _tempModel.RemoveRange(5, itemToRemove);
            }
            _tempModel = _tempModel.OrderByDescending(s => s.Rating)
                .ThenBy(s => s.Distance).ToList();
            return _tempModel[0];
        }
    }
}