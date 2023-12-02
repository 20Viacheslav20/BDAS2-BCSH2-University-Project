using Models.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Storage
    {
        public int Id { get; set; }

        [Display(Name = "Number of shelves")]
        public int NumberOfShelves { get; set; }    
        public List<StoragedProduct> Products { get; set; }  
    }
}
