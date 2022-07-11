using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Entities.Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.UserName)
                .IsUnique(); // Unique Khai báo giá trị này là duy nhất.
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.EmailAddress)
               .IsUnique(); // Unique Khai báo giá trị này là duy nhất.
        }
    }
}
