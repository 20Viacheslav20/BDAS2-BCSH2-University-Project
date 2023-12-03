using Models.Models;
using Oracle.ManagedDataAccess.Client;
using Repositories.IRepositories;

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

        public void Edit(Payment payment)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetAllPayments()
        {
            throw new NotImplementedException();
        }

        public Payment GetPayment(int id)
        {
            throw new NotImplementedException();
        }
    }
}