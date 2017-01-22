using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("Symptoms")]
    public class Symptoms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PatientType { get; set; }
        public bool HasDerivedSymptoms { get; set; }
        public int SpecialistId { get; set; }
    }
}