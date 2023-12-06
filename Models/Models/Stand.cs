using Models.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Stand
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [Display(Name = "Count of shelves")]
        public int CountOfShelves { get; set; }

        public List<StoragedProduct> Products { get; set; }
    }
}
