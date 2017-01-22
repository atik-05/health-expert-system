using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("Comment")]
    public class Comment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ServiceProviderId { get; set; }
        public int Rating { get; set; }
    }
}