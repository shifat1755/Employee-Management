using EmployeeManagement.Models;
using EmployeeManagement.Models.Employee;
using EmployeeManagement.Models.Entities;

namespace EmployeeManagement.Service.EmployeeService
{
    public interface IEmployeeService
    {
        public Task<Response<object>> addEmployee(AddEmployeeViewModel model);
        public Task<Response<List<EmployeeViewModel>>> GetAllEmployee();
        public Task<int> GetEmployeeCountAsync();
        public Task<List<Employee>> GetEmployeesAsync(int pageNumber, int pageSize);


    }
}
