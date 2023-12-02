using Models.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Storage
    {
        public int Id { get; set; }

        [Display(Name = "Number of shelves")]
        public int NumberOfShelves { get; set; }    
        public List<StoragedProduct> Products { get; set; }  
    }
}
