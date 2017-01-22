using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class RequiredDataForSuggestion
    {
        public int SymptomId { get; set; }
        public int DerivedSymptomId { get; set; }
        public bool IsEmergency { get; set; }
        public int SuggestionTypeId { get; set; }

        public List<AllDistance> AllDistances { get; set; }
    }
}