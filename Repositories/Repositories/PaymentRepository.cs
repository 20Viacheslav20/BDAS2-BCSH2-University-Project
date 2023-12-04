using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly OracleConnection _oracleConnection;

        private const string TABLE = "PLATBY";

        public PaymentRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public void Create(Payment payment)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }



        public List<Payment> GetAllPayments()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT
                                            p.idplatby IDPLATBY,
                                            p.jeclubcarta JECLUBCARTA,
                                            p.platba_type TYPPLATBY,
                                            h.vraceno VRACENOHOTOVE,
                                            k.cislokarty CISLOKARTY,
                                            k.autorizacnikod AUTORIZACNIKOD,
                                            ku.cislo CISLOKUPONU
                                        FROM
                                            {TABLE} p
                                        LEFT JOIN
                                            hotove h ON p.idplatby = h.idplatby
                                        LEFT JOIN
                                            karty k ON p.idplatby = k.idplatby
                                        LEFT JOIN
                                            kupony ku ON p.idplatby = ku.idplatby";

                List<Payment> payments = new List<Payment>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Payment payment = CreatePaymentFromReader(reader);
                        payments.Add(payment);
                    }
                    return payments;
                }
            }
        }

        public Payment GetPayment(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT
                                        p.idplatby IDPLATBY,
                                        p.jeclubcarta JECLUBCARTA,
                                        p.platba_type TYPPLATBY,
                                        h.vraceno VRACENOHOTOVE,
                                        k.cislokarty CISLOKARTY,
                                        k.autorizacnikod AUTORIZACNIKOD,
                                        ku.cislo CISLOKUPONU
                                    FROM
                                        {TABLE} p
                                    LEFT JOIN
                                        hotove h ON p.idplatby = h.idplatby
                                    LEFT JOIN
                                        karty k ON p.idplatby = k.idplatby
                                    LEFT JOIN
                                        kupony ku ON p.idplatby = ku.idplatby
                                    where p.idplatby = :idPlatby";

                command.Parameters.Add("idPlatby", OracleDbType.Int32).Value = id;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreatePaymentFromReader(reader);
                }
            }
        }
        private Payment CreatePaymentFromReader(OracleDataReader reader)
        {
            int id = int.Parse(reader["IDPLATBY"].ToString());
            bool isClubCard = Convert.ToBoolean(reader["JECLUBCARTA"]);
            string type = reader["TYPPLATBY"].ToString();

            Payment payment;

            switch (type)
            {
                case "HOTOVE":
                    int returned = int.Parse(reader["VRACENOHOTOVE"].ToString());
                    payment = new Cash
                    {
                        Id = id,
                        IsClubCard = isClubCard,
                        Returned = returned,
                        Type = type
                    };
                    break;

                case "KARTA":
                    int cardNumber = int.Parse(reader["CISLOKARTY"].ToString());
                    int authorizationCode = int.Parse(reader["AUTORIZACNIKOD"].ToString());
                    payment = new CreditCard
                    {
                        Id = id,
                        IsClubCard = isClubCard,
                        CardNumber = cardNumber,
                        AuthorizationCode = authorizationCode,
                        Type = type
                    };
                    break;

                case "KUPON":
                    int couponNumber = int.Parse(reader["CISLOKUPONU"].ToString());
                    payment = new Coupon
                    {
                        Id = id,
                        IsClubCard = isClubCard,
                        Number = couponNumber,
                        Type = type
                    };
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported payment type: {type}");
            }
            return payment;
        }
    }
}