using Models.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace Models.Models.Storage
{
    public class Storage
    {
        public Storage()
        {
            Products = new List<StoragedProduct>();
        }
        public int Id { get; set; }

        [Display(Name = "Number of shelves")]
        public int NumberOfShelves { get; set; }
        public List<StoragedProduct> Products { get; set; }
        public int ShopId { get; set; }
    }
}
