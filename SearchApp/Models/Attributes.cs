using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SearchApp.Models
{
    public class Attributes
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string FreebaseName { get; set; }
        public virtual ICollection<Types> Types { get; set; }
    }
}