using Microsoft.EntityFrameworkCore;
using StockExampleApp.Entity;

namespace StockExampleApp.Data.Concrete
{
    public class Context : DbContext
    {
        
        public Context(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
