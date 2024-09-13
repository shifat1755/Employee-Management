using EmployeeManagement.Models.Employee;
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
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddEmployeeViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel model)
        {
            var response = await _employeeService.addEmployee(model);
            if (response.isSuccessful == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
