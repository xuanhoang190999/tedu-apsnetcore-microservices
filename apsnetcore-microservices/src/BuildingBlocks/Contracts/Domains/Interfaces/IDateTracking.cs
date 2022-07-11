

namespace Contracts.Domains.Interfaces
{
    public interface IDateTracking
    {
        DateTimeOffset CreatedDate { get; set; }

        DateTimeOffset? LastModifiedDate { get; set;}
    }

    // DateTimeOffset tốt hơn DateTime cho việc convert từ UTC qua local
}
