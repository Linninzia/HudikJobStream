using Microsoft.EntityFrameworkCore;

namespace JobApi.Models
{
    public class JobContext : DbContext
    {
        public JobContext(DbContextOptions<JobContext> options)
            : base(options)
        {
        }

        public DbSet<JobItem> JobItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = JobStream;");
        }


    }

}
