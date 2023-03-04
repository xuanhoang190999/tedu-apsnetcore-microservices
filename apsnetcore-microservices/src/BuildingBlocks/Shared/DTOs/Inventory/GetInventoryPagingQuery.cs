using Shared.SeedWork;

namespace Shared.DTOs.Inventory
{
    public class GetInventoryPagingQuery : PagingRequestParameters
    {
        // Múc đích config itemNo như vậy để ẩn đi field itemNo này, khi call api sẽ không thấy và không còn thêm filed này.
        // Nếu để hiện thì người ta sẽ không biết có nên thêm field này vào hay không.

        public string ItemNo() => _itemNo;

        private string _itemNo;

        public void SetItemNo(string itemNo) => _itemNo = itemNo;

        public string? SearchTerm { get; set; }
    }
}
