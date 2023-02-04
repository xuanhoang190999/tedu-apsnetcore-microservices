using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.Product.API.Entities.Abstraction
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; protected init; }

        [BsonElement("createdDate")] // Định nghĩa tên trong MDB
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; protected init; } = DateTime.UtcNow;

        [BsonElement("lastModifiedDate")] // Định nghĩa tên trong MDB
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? LastModifiedDate { get; protected init; } = DateTime.UtcNow;
    }

    // Có thể kế thừa từ IAuditable như MDB chưa hỗ trợ DateTimeOffset, khi lưu xuống sẽ báo báo lỗi (có thể truyển vào IAuditable biến T để định nghĩa kiểu dữ liệu cho biến thời gian đó).
}
