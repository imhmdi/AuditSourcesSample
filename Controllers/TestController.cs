using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditSourcesSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TestController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Company>> CreateData()
        {
            await dbContext.Database.EnsureCreatedAsync();

            var newCompany = new Company { Name = "Honamic" };
            var employee = new Employee { Name = "iman mohammadi" };

            employee.Activities.Add(new Activity { Description = "Start" }); ;
            newCompany.Employees.Add(employee);

            dbContext.Companies.Add(newCompany);

            await dbContext.SaveChangesAsync();

            return newCompany;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<object>> GetLastCompanyAudit()
        {
            var newCompany = new Company { Name = "Honamic" };
            var employee = new Employee { Name = "iman mohammadi" };

            employee.Activities.Add(new Activity { Description = "Start" }); ;
            newCompany.Employees.Add(employee);

            dbContext.Companies.Add(newCompany);

            dbContext.SaveChanges();

            var auditSources = await dbContext.Companies.AsNoTracking()
                .Where(c => JsonDbFunctions.Value(c.CreatedSources, "$.cn") == "MyApp")
                .Select(c => new
                {
                    HostName = JsonDbFunctions.Value(c.CreatedSources, "$.hn"),
                    ClientName = JsonDbFunctions.Value(c.CreatedSources, "$.cn"),
                    ClientVersion = JsonDbFunctions.Value(c.CreatedSources, "$.cv"),
                    ApplicationName = JsonDbFunctions.Value(c.CreatedSources, "$.an"),
                    ApplicationVersion = JsonDbFunctions.Value(c.CreatedSources, "$.av"),

                })
                .ToListAsync();



            return auditSources;
        }
    }
}
