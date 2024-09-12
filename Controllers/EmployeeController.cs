using EmployeeManagement.Service.EmployeeService;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) {
            _employeeService= employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
