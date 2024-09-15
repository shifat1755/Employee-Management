using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Phone { get; set; }
        public DateOnly? DOB { get; set; }
        public string? ProfileImg { get; set; }
    }
}
