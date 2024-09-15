using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Employee;
using EmployeeManagement.Models.Entities;
using EmployeeManagement.Service.EmployeeService;
using EmployeeManagement.Service.FileService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;

        public EmployeeController(IEmployeeService employeeService, ApplicationDbContext dbContext, IFileService fileService) {
            _employeeService= employeeService;
            _dbContext = dbContext;
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
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
        [HttpPost]
        public async Task<IActionResult> GetEmployees([FromBody] PaginationViewModel model)
        {
            int pageNumber = model.page;
            int pageSize = 3;
            var totalCount = await _employeeService.GetEmployeeCountAsync();
            var employees = await _employeeService.GetEmployeesAsync(pageNumber, pageSize);

            var paginationData = new
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                PageSize = pageSize,
                TotalItems = totalCount,
                Employees = employees
            };

            return Json(paginationData);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            Guid EId = Guid.Parse(id);
            var employee=await _dbContext.Employees.FindAsync(EId);
            var model = new EditEmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                DOB = employee.DOB,
                ProfileImg = employee.ProfileImg,

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel model)
        {
            var employee = await _dbContext.Employees.FindAsync(model.Id);
            employee.Email = model.Email;
            employee.Name = model.Name;
            employee.Phone = model.Phone;
            employee.DOB = model.DOB;
            if (employee.ProfileImg != null)
            {
                await _fileService.DeletePrevImg(employee.ProfileImg);
            }
            employee.ProfileImg= await _fileService.SaveProfileImage(model.ProfileImgFile);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            Guid GId = Guid.Parse(id);
            var employee = await _dbContext.Employees.FindAsync(GId);
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] EmployeeSearchViewModel searchModel)
        {
            try
            {
                var query = _dbContext.Employees.AsQueryable();

                if (!string.IsNullOrEmpty(searchModel.DOB))
                {
                    if (DateTime.TryParse(searchModel.DOB, out DateTime dob))
                    {
                        var dobOnly = DateOnly.FromDateTime(dob);
                        query = query.Where(e => e.DOB == dobOnly);
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchModel.Phone))
                {
                    query = query.Where(e => e.Phone.Contains(searchModel.Phone));
                }
                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                {
                    var nameToLower = searchModel.Name.ToLower();
                    query = query.Where(e => e.Name.ToLower().Contains(nameToLower));
                }

                if (!string.IsNullOrWhiteSpace(searchModel.Email))
                {
                    var emailToLower = searchModel.Email.ToLower();
                    query = query.Where(e => e.Email.ToLower().Contains(emailToLower));
                }

                var employees = await query.ToListAsync();
                return Json(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




    }
}
