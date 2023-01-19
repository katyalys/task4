using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using System;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public AccountController(MVCDemoDbContext mvcDemoDbContext)
        {
          this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Password = addEmployeeRequest.Password,
                RegisterTime = DateTime.Now,
                Status = "active",
                LastLoginTime = new DateTime(),
            };

            if (mvcDemoDbContext.Employees.Any(x => x.Email == employee.Email))
            {
                ViewBag.DuplicateMessage = "This email already exists";
                return View();
            }
            else
            {
                await Authenticate(employee.Id);
                await mvcDemoDbContext.Employees.AddAsync(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Action");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (employee != null && employee.Status == "active")
                {
                    model.LastLoginTime = DateTime.Now;
                    employee.LastLoginTime = model.LastLoginTime;
                    mvcDemoDbContext.Entry(employee).State = EntityState.Modified;
                    mvcDemoDbContext.SaveChanges();

                    await Authenticate(employee.Id); // аутентификация

                    return RedirectToAction("Index", "Action");
                }
                ModelState.AddModelError("", "Incorrect login/password or you are blocked");
            }
            return View(model);
        }

        private async Task Authenticate(Guid employeeId)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employeeId.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
