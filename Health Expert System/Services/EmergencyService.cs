using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.Services
{
    public class EmergencyService
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();
        private List<SuggestionViewModel> _model;
        private List<SuggestionViewModel> _tempModel;
        private List<SuggestionViewModel> resultModel = new List<SuggestionViewModel>();
        private string _status;
        private readonly int _scope = 5;

        public EmergencyService(List<SuggestionViewModel> model)
        {
            this._model = model;
            SetPatient_status();
        }

        public List<SuggestionViewModel> GetEmergencyServiceProvider()
        {
            SortModelByDistance();

            var provider = _model[0];
            resultModel.Add(provider);

            _model.Remove(_model[0]);

            if (_status == "poor")
            {
                resultModel.Add(GetNearestGovtHospital());
            }
            else if (_status == "moderate")
            {
                resultModel.Add(GetServiceProviderForModerate());
            }
            else if (_status == "high")
            {
                resultModel.Add(GetRichServiceProvider());
            }

            return resultModel;
        }

        private void SetPatient_status()
        {
            int userId = WebSecurity.CurrentUserId;
            var patient = _dbContext.PatientProfiles.FirstOrDefault(s => s.UserId == userId);
            if (patient != null)
                _status = patient.LifestyleStatus;
        }

        private void SortModelByDistance()
        {
            _model = _model.OrderBy(m => m.Distance).ToList();
        }

        private SuggestionViewModel GetNearestGovtHospital()
        {
            _tempModel = _model;
            var govtHospital = _tempModel.FirstOrDefault(s => s.IsGovt == true);
            return govtHospital;
        }

        private SuggestionViewModel GetServiceProviderForModerate()
        {
            _tempModel = _model;
            var length = _tempModel.Count;
            var itemToRemove = length - _scope;
            if (length > _scope)
            {
                _tempModel.RemoveRange(_scope, itemToRemove);
            }
            _tempModel =
                _tempModel.OrderByDescending(s => s.Rating)
                    .ThenBy(s => s.Fee)
                    .ThenBy(s => s.Distance)
                    .ToList();

            return _tempModel[0];
        }

        private SuggestionViewModel GetRichServiceProvider()
        {
            _tempModel = _model;
            _tempModel.Remove(_tempModel[0]);
            var length = _tempModel.Count;
            var itemToRemove = length - _scope;
            if (length > _scope)
            {
                _tempModel.RemoveRange(_scope, itemToRemove);
            }
            _tempModel =
                _tempModel.OrderByDescending(s => s.Fee)
                    .ThenByDescending(s => s.Rating)
                    .ThenBy(s => s.Distance)
                    .ToList();

            return _tempModel[0];
        }

    }
}