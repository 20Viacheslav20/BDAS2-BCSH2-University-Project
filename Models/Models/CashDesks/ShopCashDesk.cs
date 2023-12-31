﻿using System.ComponentModel.DataAnnotations;


namespace Models.Models.CashDesks
{
    public class ShopCashDesk
    {
        public int Id { get; set; }

        [Display(Name = "Cash desk number")]
        public int Number { get; set; }

        [Display(Name = "Is self")]
        public bool IsSelf { get; set; }
        public int ShopId { get; set; }
    }
}
