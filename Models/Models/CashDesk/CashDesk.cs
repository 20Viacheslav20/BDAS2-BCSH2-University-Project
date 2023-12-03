using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.CashDesk
{
    public class CashDesk
    {
        public int Id { get; set; }

        [Display(Name = "Cash desk number")]
        public int Count { get; set; }

        [Display(Name = "Is self")]
        public bool isSelf { get; set; }
    }
}
