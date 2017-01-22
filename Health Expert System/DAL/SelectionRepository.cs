using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Fileters;
using Health_Expert_System.Models;
using Health_Expert_System.Models.ViewModels;
using WebMatrix.WebData;

namespace Health_Expert_System.DAL
{   
    public class SelectionRepository
    {
        private DatabaseContext _database = new DatabaseContext();
        private int userId;

        public SelectionRepository(int userId)
        {
            this.userId = userId;
        }

        public SelectionRepository()
        {
        }

        public void SaveSelection(int id, int diseaseId, float latitude, float longitude)
        {
            var currentDate = DateTime.Today;
            var userId = WebSecurity.CurrentUserId;
            var history = new SelectionHistory
            {
                PatientId = userId,
                ServiceProviderId = id,
                DiseaseId = diseaseId,
                PatientLat = latitude,
                PatientLng = longitude,
                IsCommented = false,
                Date = currentDate.ToString("MMMM dd, yyyy")
            };
            _database.SelectionHistories.Add(history);
            _database.SaveChanges();
        }

        public List<SelectionHistory> GetSelectionHistories()
        {
            return _database.SelectionHistories.ToList();
        }

        public void CommentOnHistory(int historyId)
        {
            var history = _database.SelectionHistories.Find(historyId);
            history.IsCommented = true;
            _database.SaveChanges();
        }

        public List<HomePageViewModel> GetNotRatedProvider()
        {
            var histories = _database.SelectionHistories.Where(s => s.PatientId == userId && s.IsCommented == false);
            
            var result = from history in histories
                join serviceProvider in _database.ServiceProviders on history.ServiceProviderId equals serviceProvider.Id
                join disease in _database.Symptomses on history.DiseaseId equals disease.Id
                select new HomePageViewModel
                {
                    Id = history.Id,
                    ServiceProviderId = serviceProvider.Id,
                    ServiceProvider = serviceProvider.Name,
                    Disease = disease.Name,
                    Date = history.Date
                };
            return result.ToList();
        }

        public List<HomePageViewModel> GetRecentTreatmentHistory()
        {
            var histories = _database.SelectionHistories.Where(s=>s.PatientId == userId).ToList();
            var length = histories.Count;
            
            var result = (from history in histories
                         join serviceProvider in _database.ServiceProviders on history.ServiceProviderId equals serviceProvider.Id
                         join disease in _database.Symptomses on history.DiseaseId equals disease.Id
                         select new HomePageViewModel
                         {
                             Id = history.Id,
                             ServiceProvider = serviceProvider.Name,
                             Disease = disease.Name,
                             Date = history.Date,

                             PatientId = history.PatientId,
                             ServiceProviderId = history.ServiceProviderId
                         }).ToList();
           
            int i = 0;
            foreach (var model in result)
            {
                var history =
                    _database.Comments.FirstOrDefault(
                        s => s.PatientId == model.PatientId && s.ServiceProviderId == model.ServiceProviderId);
                if (history!=null)
                {
                    result[i].Rating = history.Rating;
                }
                i++;
            }
            return result.ToList();
        }
    }
}