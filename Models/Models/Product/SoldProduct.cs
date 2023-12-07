using System.ComponentModel.DataAnnotations;


namespace Models.Models.Product
{
    public class SoldProduct
    {
        [Display(Name = "Products count")]
        public int ProductsCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale.")]
        public int SaleId { get; set; }
        public int ProductId { get; set; }

        [Display(Name = "Sold Price")]
        public int SoldPrice { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }
       
        [Display(Name = "Sold Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SoldDate {  get; set; }
    }
}
