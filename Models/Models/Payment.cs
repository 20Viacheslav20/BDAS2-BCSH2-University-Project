using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Is ClubCard")]
        public bool IsClubCard { get; set; }
        public PaymentType Type { get; set; }
    }

    public class Coupon : Payment
    {
        public Coupon()
        {
            Type = PaymentType.KUPON;
        }
        public int Number { get; set; }
    }

    public class Cash : Payment
    {
        public Cash()
        {
            Type = PaymentType.HOTOVE;
        }
        public int Returned { get; set; }
    }

    public class CreditCard : Payment
    {
        public CreditCard()
        {
            Type = PaymentType.KARTA;
        }
        [Display(Name = "Card Number")]
        public int CardNumber { get; set; }

        [Display(Name = "Authorization Code")]
        public int AuthorizationCode { get; set; }
    }

    public enum PaymentType
    {
        HOTOVE,
        KARTA,
        KUPON
    }
}