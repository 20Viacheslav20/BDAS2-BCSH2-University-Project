using Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DateNotInFuture(ErrorMessage = "The sale date cannot be in the future.")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Total Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid number.")]
        public int TotalPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid payment. If you don't have any payments, you need to create a new one.")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public string Displayinfo => Payment == null ? "" : $"{SaleDate.ToString("dd.MM.yyyy")} | {TotalPrice} | {Payment.Type}";
    }
}
