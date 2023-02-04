namespace Shared.Enums
{
    public enum EDocumentType
    {
        All = 0, // Filter, trường hợp tìm kiếm tất cả loại giao dịch
        Purchase = 101, // Chỉ tìm loại mua hàng
        PurchaseInternal = 102, // Mua nội bộ, tự xí nghiệp này qua xí nghiệp khác
        Sale = 201, // Loại bán hàng
        SaleInternal = 202, // Mua nội bộ, trong doanh nghiệp
    }
}
