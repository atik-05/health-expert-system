using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("DoctorProfile")]
    public class DoctorProfile 
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public int RegistrationNumber { get; set; }
        public int SpecialistId { get; set; }
        public int YearOfExperience { get; set; }
        public int VisitingFee { get; set; }
        public string StartAndEndTime { get; set; }
    }
}