
using System.ComponentModel.DataAnnotations;

namespace Models.Models.Storage
{
    public class Order
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid storage.")]
        public int StorageId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product.")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid number.")]
        public int ProductCount { get; set; }
    }
}
