using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;

namespace Health_Expert_System.DAL
{
    public class RatingRepository
    {
        private DatabaseContext _databaseContext = new DatabaseContext();

        public List<Comment> GetAllRating()
        {
            var allRatings = _databaseContext.Comments.ToList();
            return allRatings;
        }

        public void AddRating(Comment comment)
        {
            _databaseContext.Comments.Add(comment);
            _databaseContext.SaveChanges();
        }

        public void EditRating(Comment comment)
        {
            var com =
                _databaseContext.Comments.FirstOrDefault(
                    s => s.PatientId == comment.PatientId && s.ServiceProviderId == comment.ServiceProviderId);
            if (com!=null)
            {
                com.Rating = comment.Rating;
            }
            _databaseContext.SaveChanges();
        }

        public List<int> GetProviderRatings(int providerId)
        {
            var ratings = (from comment in _databaseContext.Comments
                where comment.ServiceProviderId == providerId
                select comment.Rating).ToList();
            return ratings;
        }

        public bool IsCommented(int patientId, int providerId)
        {
            var comment =
                _databaseContext.Comments.FirstOrDefault(
                    s => s.PatientId == patientId && s.ServiceProviderId == providerId);
            if (comment==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}