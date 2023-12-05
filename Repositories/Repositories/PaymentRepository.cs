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
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = "INSERT INTO PLATBY (jeclubcarta, platba_type) VALUES (:jeclubcarta, :platbaType)";

                command.Parameters.Add(":isClubCard", OracleDbType.Int16).Value = payment.IsClubCard ? 1 : 0;
                command.Parameters.Add(":platbaType", OracleDbType.Varchar2).Value = payment.Type;

                command.ExecuteNonQuery();

                command.CommandText = "SELECT idplatby FROM platby WHERE ROWNUM = 1 ORDER BY idplatby DESC";

                int idPlatby = Convert.ToInt32(command.ExecuteScalar());

                if (idPlatby > 1)
                {
                    if (payment.Type == PaymentType.HOTOVE)
                    {
                        Cash cashPayment = payment as Cash;

                        command.CommandText = "INSERT INTO HOTOVE (IDPLATBY, VRACENO) VALUES (:idPlatby, :cashPaymentReturned)";

                        command.Parameters.Clear();

                        command.Parameters.Add(":idPlatby", OracleDbType.Int32).Value = idPlatby;
                        command.Parameters.Add(":cashPaymentReturned", OracleDbType.Int32).Value = cashPayment.Returned;
                    }
                    else if (payment.Type == PaymentType.KARTA)
                    {
                        CreditCard creditCardPayment = payment as CreditCard;

                        command.CommandText = @"INSERT INTO KARTY (IDPLATBY, CISLOKARTY, AUTORIZACNIKOD) 
                                                VALUES (:idPlatby, :creditCardPaymentCardNumber, :creditCardPaymentAuthorizationCode)";

                        command.Parameters.Clear();

                        command.Parameters.Add(":idPlatby", OracleDbType.Int32).Value = idPlatby;
                        command.Parameters.Add(":creditCardPaymentCardNumber", OracleDbType.Int32).Value = creditCardPayment.CardNumber;
                        command.Parameters.Add(":creditCardPaymentAuthorizationCode", OracleDbType.Int32).Value = creditCardPayment.AuthorizationCode;
                    }
                    else if (payment.Type == PaymentType.KUPON)
                    {
                        Coupon couponPayment = payment as Coupon;

                        command.CommandText = "INSERT INTO KUPONY (IDPLATBY, CISLO) VALUES (:idPlatby, :couponPaymentNumber)";

                        command.Parameters.Clear();

                        command.Parameters.Add(":idPlatby", OracleDbType.Int32).Value = idPlatby;
                        command.Parameters.Add(":couponPaymentNumber", OracleDbType.Int32).Value = couponPayment.Number;
                    }

                    command.ExecuteNonQuery();
                }  

            }
        }

        public void Delete(Payment payment)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = $"DELETE FROM {payment.Type} WHERE idplatby = :paymentId";
                command.Parameters.Add("paymentId", OracleDbType.Int32).Value = payment.Id;

                command.ExecuteNonQuery();

                command.Parameters.Clear();

                command.CommandText = $"DELETE FROM {TABLE} WHERE idplatby = :paymentId";
                command.Parameters.Add("paymentId", OracleDbType.Int32).Value = payment.Id;

                command.ExecuteNonQuery();
            }
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
                        Type = (PaymentType) Enum.Parse(typeof(PaymentType), type)
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
                        Type = (PaymentType)Enum.Parse(typeof(PaymentType), type)
                    };
                    break;

                case "KUPON":
                    int couponNumber = int.Parse(reader["CISLOKUPONU"].ToString());
                    payment = new Coupon
                    {
                        Id = id,
                        IsClubCard = isClubCard,
                        Number = couponNumber,
                        Type = (PaymentType)Enum.Parse(typeof(PaymentType), type)
                    };
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported payment type: {type}");
            }
            return payment;
        }
    }
}