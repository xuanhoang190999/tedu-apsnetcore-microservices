using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        public DbSet<CatalogProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vì productNo là duy nhất và coi như 1 khóa chính nên sẽ thêm thuộc tính cho productNo ở phần này. Khi tạo mới sẽ kiểm tra phần này.
            modelBuilder.Entity<CatalogProduct>().HasIndex(x => x.No)
                .IsUnique(); // Unique Khai báo giá trị này là duy nhất.
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                    e.State == EntityState.Added ||
                    e.State == EntityState.Deleted);

            foreach(var item in modified)
            {
                switch(item.State)
                {
                    case EntityState.Added:
                        if(item.Entity is IDateTracking addedEntity) // Nếu entity có triển khai IDateTracking.
                        {
                            addedEntity.CreatedDate = DateTime.UtcNow;
                            item.State = EntityState.Added;
                        }
                        break;
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false; // Nếu Modified có filed Id thì không cho sửa.
                        if (item.Entity is IDateTracking modifiedEntity) 
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State= EntityState.Modified;
                        }
                        break;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
