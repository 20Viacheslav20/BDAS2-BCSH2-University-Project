using System.ComponentModel.DataAnnotations;

namespace Models.Models.Product
{
    public class ProductStats
    {
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Storage Count")]
        public int StorageCount { get; set; }

        [Display(Name = "Stand Count")]
        public int StandCount { get; set; }
    }
}
