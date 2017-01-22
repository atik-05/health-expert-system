using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Health_Expert_System.Models
{
    [Table("Specialists")]
    public class Specialists
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}