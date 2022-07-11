using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Reponsitories.Interfaces;

namespace Product.API.Reponsitories
{
    public class ProductRepository : RepositoryBase<CatalogProduct, long, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

        public Task<CatalogProduct?> GetProduct(long id) => GetByIdAasync(id);

        public Task<CatalogProduct?> GetProductById(long id) => GetByIdAasync(id);

        public Task<CatalogProduct?> GetProductByNo(string productNo) =>
            FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync(); // SingleOrDefaultAsync: Đưa ra 1 ngoại lệ (throws an exception) nếu có nhiều hơn 1 kết quả phù hợp được tìm thấy.

        public Task CreateProduct(CatalogProduct product) => CreateAsync(product);

        public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);

        public async Task DeleteProduct(long id)
        {
            var product = await GetProduct(id);
            if(product != null) await DeleteAsync(product);
        }
    }
}
