using Contracts.Domains.Interfaces;
using Infrastructure.Common.Models;
using Inventory.Product.API.Entities;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Services.Interfaces
{
    public interface IInventoryService : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        Task<PagedList<InventoryEntryDto>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);
        Task<InventoryEntryDto> GetByIdAsync(string id);
        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
    }
}
