
namespace Models.Models
{
    public class Payment
    {
        public int Id { get; set; }
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
        public int CardNumber { get; set; }

        public int AuthorizationCode { get; set; }
    }
}