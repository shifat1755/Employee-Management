using EmployeeManagement.Data;

namespace EmployeeManagement.Service.EmployeeService
{
    public class EmployeeService:IEmployeeService
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeService(ApplicationDbContext dbContext) {
            _dbContext=dbContext;
        }


    }
}
