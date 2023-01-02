using EmployeeData.DAL;
using EmployeeData.Models;
using EmployeeData.Models.DBEntities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeData.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeeController(EmployeeDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var EmployeeViewModel = new EmployeeViewModel()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DateOfBirth = employee.DateOfBirth,
                        Email = employee.Email,
                        Salary = employee.Salary,
                    };
                    employeeList.Add(EmployeeViewModel);
                }
                return View(employeeList);
            }
            return View(employeeList);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new Employee()
                    {
                        FirstName = employeeData.FirstName,
                        LastName = employeeData.LastName,
                        DateOfBirth = employeeData.DateOfBirth,
                        Email = employeeData.Email,
                        Salary = employeeData.Salary,
                    };
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Employee created succesfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Model data is not valid";
                    return View();
                }
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                var employee = _context.Employees.SingleOrDefault(x => x.Id == Id);
                if (employee != null)
                {
                    var employeeView = new EmployeeViewModel()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DateOfBirth = employee.DateOfBirth,
                        Email = employee.Email,
                        Salary = employee.Salary,
                    };
                    return View(employeeView);

                }
                else
                {
                    TempData["errorMessage"] = $"Employee details not avaliable with the Id: ${Id}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

        }
    }
}
