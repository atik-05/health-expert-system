using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class SuggestionViewModel
    {
        public int ServiceProviderId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Specialist { get; set; }
        public string Address { get; set; }
        public int Fee { get; set; }
        public int Rating { get; set; }

        public float Distance { get; set; }
        public bool IsGovt { get; set; }
    }
}