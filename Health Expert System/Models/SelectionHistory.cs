using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("SelectionHistory")]
    public class SelectionHistory
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ServiceProviderId { get; set; }
        public int DiseaseId { get; set; }
        public double PatientLat { get; set; }
        public double PatientLng { get; set; }
        public bool IsCommented { get; set; }
        public string Date { get; set; }
    }
}