using Models.Models;

namespace Repositories.IRepositories
{
    public interface IAddressRepository
    {
        List<Address> GetAll();
        Address GetById(int id);
        void Create(Address address);
        void Edit(Address address);
        void Delete(int id);
        List<Address> GetAddressesForEmployee(int employeeId);
        List<Address> GetAddressesForShop(int shopId);
        Address GetEmployeeAddress(int employeeId);
        void SetAddressToEmployee(int employeeId, int addressId);
        Address GetShopAddress(int shopId);
        void SetAddressToShop(int shopId, int addressId);
    }
}
