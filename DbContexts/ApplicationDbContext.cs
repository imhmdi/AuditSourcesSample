using Microsoft.EntityFrameworkCore;

namespace AuditSourcesSample
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseJsonDbFunctions();

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Activity> Activities { get; set; }
    }
}