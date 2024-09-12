using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        public DateOnly DOB { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfileImg { get; set; }
    }
}
