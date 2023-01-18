using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using System;
using WebApplication1.Models;
using WebApplication1.Models.Domain;
using WebApplication1.Controllers.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
          this.mvcDemoDbContext = mvcDemoDbContext;
        }

        // displai list of employees
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Add(AddEmployeeViewModel addEmployeeRequest)
        //{

        //    using (Models db = new DBModels())
        //        if (ModelState.IsValid)
        //            return Content($"{addEmployeeRequest.Name} - {addEmployeeRequest.ConfirmPassword}");

        //    return View(addEmployeeRequest);
        //}

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            addEmployeeRequest.RegisterTime = DateTime.Now;
            addEmployeeRequest.Status = "active";

            var dat1 = new DateTime();
            addEmployeeRequest.LastLoginTime = dat1;

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Password = addEmployeeRequest.Password,
                RegisterTime = addEmployeeRequest.RegisterTime,
                Status = addEmployeeRequest.Status,
                LastLoginTime = addEmployeeRequest.LastLoginTime,
            };

            if (mvcDemoDbContext.Employees.Any(x => x.Email == employee.Email))
            {
                ViewBag.DuplicateMessage = "This email already exists";
                return View("Add");
            }
            else
            {
                await mvcDemoDbContext.Employees.AddAsync(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [HttpPost("DeleteUsers")]
        public IActionResult DeleteUsers(string[] ids)
        {
            //string[] ids = formCollection["employeeId"].ToString().Split(new char[] {','});
            foreach (string id in ids)
            {
                var deletedEmployee = mvcDemoDbContext.Employees.Find(Guid.Parse(id));
                mvcDemoDbContext.Employees.Remove(deletedEmployee);
                mvcDemoDbContext.SaveChanges();
            }
            //var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync();
            //mvcDemoDbContext.Employees.Remove(employee);
            //mvcDemoDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
