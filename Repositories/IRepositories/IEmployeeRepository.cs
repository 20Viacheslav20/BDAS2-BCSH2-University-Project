
using Models.Models;

namespace Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        void Create(Employee employee);
        void Edit(Employee employee);
        void Delete(int id);
        List<Employee> GetEmployeesWithoutAuth();
    }
}
