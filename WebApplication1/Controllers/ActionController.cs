using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [Authorize]
    public class ActionController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public ActionController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpPost("DeleteUsers")]
        public IActionResult DeleteUsers([FromBody] string[] ids)
        {
            foreach (string id in ids)
            {
                var deletedEmployee = mvcDemoDbContext.Employees.Find(Guid.Parse(id));

                if (deletedEmployee is null)
                    continue;

                mvcDemoDbContext.Employees.Remove(deletedEmployee);
                mvcDemoDbContext.SaveChanges();
            }
            return Ok();
        }

        [HttpPost("BlockUsers/{isBlock}")]
        public IActionResult BlockUsers([FromBody] string[] ids, [FromRoute] bool isBlock)
        {
            foreach (string id in ids)
            {
                var blockedEmployee = mvcDemoDbContext.Employees.Find(Guid.Parse(id));

                if (blockedEmployee is null)
                    continue;

                if (isBlock)
                    blockedEmployee.Status = "blocked";
                else
                    blockedEmployee.Status = "active";
                mvcDemoDbContext.Entry(blockedEmployee).State = EntityState.Modified;
                mvcDemoDbContext.SaveChanges();
            }
            return Ok();

        }
    }
}
