using Contracts.Common.Interfaces;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Reponsitories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<CatalogProduct, long>
    {
        Task<IEnumerable<CatalogProduct>> GetProducts();
        Task<CatalogProduct?> GetProductById(long id);
        Task<CatalogProduct?> GetProductByNo(string productNo);
        Task CreateProduct(CatalogProduct product);
        Task UpdateProduct(CatalogProduct product);
        Task DeleteProduct(long id);
    }
}
