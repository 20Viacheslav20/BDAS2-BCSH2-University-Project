using Models.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace Models.Models.Stands
{
    public class Stand
    {
        public Stand()
        {
            Products = new List<StoragedProduct>();
        }
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid value.")]
        public int Number { get; set; }

        [Display(Name = "Count of shelves")]
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid value.")]
        public int CountOfShelves { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid shop.")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public List<StoragedProduct> Products { get; set; }
    }
}
