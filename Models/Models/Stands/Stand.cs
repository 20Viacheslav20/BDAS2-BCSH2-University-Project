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

        public int Number { get; set; }

        [Display(Name = "Count of shelves")]
        public int CountOfShelves { get; set; }

        public List<StoragedProduct> Products { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
