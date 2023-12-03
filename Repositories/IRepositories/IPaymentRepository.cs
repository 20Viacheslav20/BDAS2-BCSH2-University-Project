using Models.Models;


namespace Repositories.IRepositories
{
    public interface IPaymentRepository
    {
        List<Payment> GetAllPayments();
        Payment GetPayment(int id);
        void Create(Payment payment);
        void Edit(Payment payment);
        void Delete(int id);
    }
}