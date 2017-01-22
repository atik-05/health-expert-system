using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models.ViewModels
{
    public class PatientRegistrationViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public int MonthlyIncome { get; set; }
        public string LifestyleStatus { get; set; }
    }
}