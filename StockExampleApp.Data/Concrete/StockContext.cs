using Microsoft.EntityFrameworkCore;
using StockExampleApp.Entity;

namespace StockExampleApp.Data.Concrete
{
    public class StockContext:DbContext
    {
        public StockContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        
    }
}
