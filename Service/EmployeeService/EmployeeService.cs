using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Employee;
using EmployeeManagement.Models.Entities;
using EmployeeManagement.Service.FileService;

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


    }
}
