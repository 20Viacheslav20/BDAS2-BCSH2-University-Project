using System.ComponentModel.DataAnnotations;


namespace Models.Models.Product
{
    public class SoldProduct
    {
        [Display(Name = "Products count")]
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid value.")]
        public int ProductsCount { get; set; }

        [Display(Name = "Sold Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid number.")]
        public int SoldPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale. If you don't have any sales, you need to create a new one.")]
        public int SaleId { get; set; }

        [Display(Name = "Sold Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SoldDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product.")]
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }
       
    }
}
