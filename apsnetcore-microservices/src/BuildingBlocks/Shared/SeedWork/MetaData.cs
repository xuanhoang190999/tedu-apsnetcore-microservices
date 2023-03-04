namespace Shared.SeedWork
{
    // Phân trang
    public class MetaData
    {
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public int PageSize { get; set; } // Tổng số trang

        public long TotalItems { get; set; } // Tổng item

        public bool HasPrevious => CurrentPage > 1; // Có về trước được hay không

        public bool HasNext => CurrentPage < TotalPages; // Có next được hay không

        public int FirstRowOnPage => TotalItems > 0 ? (CurrentPage - 1) * PageSize + 1 : 0; // First Row trên trang nào

        public int LastRowOnPage => (int)Math.Min(CurrentPage * PageSize, TotalItems); // Last Row trên trang nào
    }
}
