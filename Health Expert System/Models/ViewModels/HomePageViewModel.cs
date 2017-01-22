using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class HomePageViewModel
    {
        public int Id { get; set; }
        public string ServiceProvider { get; set; }
        public string Date { get; set; }
        public string Disease { get; set; }
        public int Rating { get; set; }

        public int PatientId { get; set; }
        public int ServiceProviderId { get; set; }

    }
}