using System.ComponentModel.DataAnnotations;

namespace Models.Models.Categor
{
    public class Stats
    {
        [Display(Name = "Category")]
        public string CategoryName {  get; set; }

        [Display(Name = "Total Sales")]
        public int TotalSales {  get; set; }
    }
}
