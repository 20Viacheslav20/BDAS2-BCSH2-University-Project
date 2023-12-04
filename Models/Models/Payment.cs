using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Is ClubCard")]
        public bool IsClubCard { get; set; }
        public string Type { get; set; }
    }

    public class Coupon : Payment
    {
        public int Number { get; set; }
    }

    public class Cash : Payment
    {
        public int Returned { get; set; }
    }

    public class CreditCard : Payment
    {
        [Display(Name = "Card Number")]
        public int CardNumber { get; set; }

        [Display(Name = "Authorization Code")]
        public int AuthorizationCode { get; set; }
    }
}