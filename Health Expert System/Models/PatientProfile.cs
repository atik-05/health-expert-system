using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("PatientProfile")]
    public class PatientProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public int MonthlyIncome { get; set; }
        public string LifestyleStatus { get; set; }
    }
}