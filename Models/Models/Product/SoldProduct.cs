using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Product
{
    public class SoldProduct
    {
        public int ProductsCount { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int SoldPrice { get; set; }
        public string ProductName { get; set; }
        public string PaymentType { get; set; }
       
        [Display(Name = "Sold Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SoldDate {  get; set; }


    }
}
