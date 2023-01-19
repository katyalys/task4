using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Filters
{
    public class AuthorizationFilter: Attribute, IActionFilter, IFilterMetadata
    {

        private readonly MVCDemoDbContext mvcDemoDbContext;
        public AuthorizationFilter(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        public void OnActionExecuted(ActionExecutedContext actionContext)
        {
            //  throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var principal = actionContext.HttpContext.User.Identity;

            if (mvcDemoDbContext.Employees.Select(x => x.Id.ToString()).Contains(principal.Name))
            {
                var employee = mvcDemoDbContext.Employees.FirstOrDefault(x => x.Id == Guid.Parse(principal.Name));
                if (employee.Status == "blocked")
                {
                    actionContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    actionContext.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
        }
    }
}
