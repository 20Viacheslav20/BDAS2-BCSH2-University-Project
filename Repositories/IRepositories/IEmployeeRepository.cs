using BDAS2_BCSH2_University_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IEmployeeRepository<Employee>
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        void Create(Employee employee);
        void Edit(Employee  employee);
        void Delete(int id);
        List<Employee> GetEmployeesWithoutAuth(int id);   
        void GetEmployer(int id);   
    }
}
