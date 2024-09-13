using EmployeeManagement.Models;
using EmployeeManagement.Models.Employee;

namespace EmployeeManagement.Service.EmployeeService
{
    public interface IEmployeeService
    {
        public Task<Response<object>> addEmployee(AddEmployeeViewModel model);
    }
}
