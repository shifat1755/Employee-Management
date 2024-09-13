using EmployeeManagement.Models.Employee;

namespace EmployeeManagement.Service.FileService
{
    public interface IFileService
    {
        Task<string> SaveProfileImage(IFormFile file);
    }
}
