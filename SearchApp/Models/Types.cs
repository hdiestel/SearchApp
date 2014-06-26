using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SearchApp.Models
{
    public class Types
    {
        public int ID {get; set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public string FreebaseName { get; set; }
        public virtual ICollection<Attributes> Attributes { get; set; }
        public virtual ICollection<Domains> Domains { get; set; }

    }
}