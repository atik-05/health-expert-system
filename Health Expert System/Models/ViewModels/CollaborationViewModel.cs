using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class CollaborationViewModel
    {
        public int HistoryId { get; set; }
        public int PatientId { get; set; }
        public int ServiceProviderId { get; set; }
        public string Type { get; set; }
        public float Distance { get; set; }
        public float StandardDistance { get; set; }
        public int Age { get; set; }
        public int Income { get; set; }

        public float EuclideanDistance { get; set; }
    }
}