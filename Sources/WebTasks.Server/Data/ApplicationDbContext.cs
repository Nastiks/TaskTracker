using Microsoft.EntityFrameworkCore;
using WebTasks.Shared.Models;

namespace WebTasks.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<ProjectItem> ProjectItems { get; set; }
    }
}
