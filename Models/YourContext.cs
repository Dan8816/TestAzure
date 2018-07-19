using Microsoft.EntityFrameworkCore;//add this dependency

namespace Products.Models
{
    public class YourContext : DbContext
    {
        public YourContext(DbContextOptions<YourContext> options) : base(options) { }
        public DbSet<Product> products { get; set; } //Users = the table name
        //<Person> is the class model that will link to the database
        public DbSet<Category> categories { get; set; }
        public DbSet<Groupings> categorized { get; set; }
    }
}