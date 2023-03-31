using Inventory.Grpc.Client;

namespace Basket.API.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _stockProtoService;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoService)
        {
            _stockProtoService = stockProtoService ?? throw new ArgumentNullException(nameof(stockProtoService));
        }

        // Note: Phải ghi đúng tên hàm GetStock.
        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
                return await _stockProtoService.GetStockAsync(stockItemRequest);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
