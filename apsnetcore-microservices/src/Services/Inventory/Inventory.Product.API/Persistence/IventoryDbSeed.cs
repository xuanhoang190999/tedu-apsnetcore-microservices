using Inventory.Product.API.Entities;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.Enums;

namespace Inventory.Product.API.Persistence
{
    public class IventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient mongoClient, MongoDBSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryEntries");
            if(await inventoryCollection.EstimatedDocumentCountAsync() == 0) // Nếu trong DB chưa có dòng dữ liệu nào
            {
                await inventoryCollection.InsertManyAsync(GetPreconfiguredInventories());
            }
        }

        private IEnumerable<InventoryEntry> GetPreconfiguredInventories()
        {
            return new List<InventoryEntry>
        {
            new()
            {
                Quantity = 10,
                DocumentNo = Guid.NewGuid().ToString(),
                ItemNo = "Lotus",
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = EDocumentType.Purchase
            },
            new()
            {
                ItemNo = "Cadillac",
                Quantity = 10,
                DocumentNo = Guid.NewGuid().ToString(),
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = EDocumentType.Purchase
            },
        };
        }
    }
}
