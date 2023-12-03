using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Stand
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [Display(Name = "Count of shelves")]
        public int CountOfShelves { get; set; }
    }
}
