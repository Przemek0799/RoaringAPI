using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger

namespace RoaringAPI.Model
{
    public class RoaringDbcontext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CompanyStructure> CompanyStructures { get; set; }
        public DbSet<FinancialRecord> FinancialRecords { get; set; }
        public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
        public DbSet<CompanyRating> CompanyRatings { get; set; }

        // Add a logger property
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           

            // Set up the logger factory
            options.UseLoggerFactory(MyLoggerFactory);

            // Use SQLite database
            options.UseSqlite("Data Source=Roaring.db");
        }
       

    }
}
