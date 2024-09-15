using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Employee;
using EmployeeManagement.Models.Entities;
using EmployeeManagement.Service.FileService;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Service.EmployeeService
{
    public class EmployeeService:IEmployeeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;

        public EmployeeService(ApplicationDbContext dbContext, IFileService fileService) {
            _dbContext=dbContext;
            _fileService=fileService;
        }

        public async Task<Response<object>> addEmployee(AddEmployeeViewModel model)
        {
            var response = new Response<object>();
            try
            {
                if (model.ProfileImgFile != null)
                {
                    model.ProfileImg = await _fileService.SaveProfileImage(model.ProfileImgFile);
                }
                var employee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    DOB = model.DOB,
                    Phone = model.Phone
                };

                if (model.ProfileImg != null) { 
                    employee.ProfileImg=model.ProfileImg;
                }
                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();
                response.isSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                response.Message = ex.Message;
                response.isSuccessful = false;
            }
            return response;
        }
        public async Task<Response<List<EmployeeViewModel>>> GetAllEmployee()
        {
            var response = new Response<List<EmployeeViewModel>>();
            try
            {
                var users = await _dbContext.Employees.Select(user => new EmployeeViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone =user.Phone,
                    DOB = user.DOB,
                    ProfileImg = user.ProfileImg,
                }).ToListAsync();
                response.Data = users;
                response.isSuccessful = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                response.isSuccessful = false;
            }
            return response;
        }
        public async Task<List<Employee>> GetEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Employees
                .OrderBy(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<int> GetEmployeeCountAsync()
        {
            return await _dbContext.Employees.CountAsync();
        }



    }
}
