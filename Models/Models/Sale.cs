using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Total Price")]
        public int TotalPrice { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public string Displayinfo => $"{SaleDate.ToString("dd.MM.yyyy")} | {TotalPrice} | {Payment.Type}";
    }
}
