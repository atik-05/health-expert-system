using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("ServiceProvider")]
    public class ServiceProvider
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Rating { get; set; }
    }
}