using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Localiz.Models
{
    public class CustomersViewModel
    {
        [Display(Name ="Name")]
        public string Name { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }
    }
}
