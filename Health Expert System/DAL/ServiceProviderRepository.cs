using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Health_Expert_System.Models;

namespace Health_Expert_System.DAL
{
    public class ServiceProviderRepository
    {

        readonly DatabaseContext _databaseContext = new DatabaseContext();

        public List<ServiceProvider> GetServiceProviders()
        {
            return _databaseContext.ServiceProviders.ToList();
        }

        public int AddServiceProvider(ServiceProvider serviceProvider)
        {
            _databaseContext.ServiceProviders.Add(serviceProvider);
            _databaseContext.SaveChanges();
            return serviceProvider.Id;
        }

        public void UpdateRating(int providerId, int rating)
        {
            var provider = _databaseContext.ServiceProviders.Find(providerId);
            provider.Rating = rating;
            _databaseContext.SaveChanges();
        }


    }
}