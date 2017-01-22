using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("HospitalProfile")]
    public class HospitalProfile
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public int HospitalTypeId { get; set; }
        public string ListOfSpecialistId { get; set; }
        public int CabinFee { get; set; }
        public bool IsGovt { get; set; }
    }
}