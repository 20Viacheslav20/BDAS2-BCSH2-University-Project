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
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid value")]
        public int NumberOfShelves { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid shop. If you don't have any shops, you need to create a new one.")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public List<StoragedProduct> Products { get; set; }
    }
}
