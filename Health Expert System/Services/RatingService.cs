using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.DAL;
using Health_Expert_System.Models;

namespace Health_Expert_System.Services
{
    public class RatingService
    {
        private RatingRepository _ratingRepository = new RatingRepository();
        private ServiceProviderRepository _providerRepository = new ServiceProviderRepository();
        private SelectionRepository _selectionRepository = new SelectionRepository();
        private int patientId, providerId, rating, selectionHistoryId;



        public RatingService(Comment comment)
        {
            this.patientId = comment.PatientId;
            this.providerId = comment.ServiceProviderId;
            this.rating = comment.Rating;
            this.selectionHistoryId = comment.Id;
        }
        public void StoreRating()
        {
            var isCommented = _ratingRepository.IsCommented(patientId, providerId);
            if (isCommented)
            {
                var comment = CreateCommentModel();
                _ratingRepository.EditRating(comment);
            }
            else
            {
                var comment = CreateCommentModel();
                _ratingRepository.AddRating(comment);
                _selectionRepository.CommentOnHistory(selectionHistoryId);
            }

            int updatedRating = CalculateWeightedAverage();
            _providerRepository.UpdateRating(providerId, updatedRating);

        }

        public Comment CreateCommentModel()
        {
            var comment = new Comment
            {
                PatientId = patientId,
                ServiceProviderId = providerId,
                Rating = rating
            };
            return comment;
        }

        public int CalculateWeightedAverage()
        {
            var ratings = _ratingRepository.GetProviderRatings(providerId);
            ratings.Add(rating);

            int count = ratings.Count;
            int distinct = ratings.Distinct().Count();

            int[] arrOfDistinctRating = new int[distinct];
            int[] arrOfWeight = new int[distinct];

            int j = 0;
            int weight = 1;
            arrOfDistinctRating[j] = ratings[j];
            arrOfWeight[j] = weight;
            j++;
            

            for (int i = 1; i < count; i++)
            {
                if (ratings[i-1]==ratings[i])
                {
                    weight++;
                }
                else
                {
                    arrOfDistinctRating[j] = ratings[i];
                    arrOfWeight[j - 1] = weight;
                    weight = 1;
                    arrOfWeight[j] = weight;
                    j++;
                }
            }
            int sumOfProduct=0;
            int sumOfWeight=0;
            for (int i = 0; i < distinct; i++)
            {
                sumOfProduct += arrOfDistinctRating[i]*arrOfWeight[i];
                sumOfWeight += arrOfWeight[i];
            }
            var finalRating = sumOfProduct/sumOfWeight;

            return finalRating;
        }
    }
}