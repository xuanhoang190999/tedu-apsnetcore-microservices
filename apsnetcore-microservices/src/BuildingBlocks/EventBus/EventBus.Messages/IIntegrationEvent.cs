namespace EventBus.Messages
{
    public interface IIntegrationEvent
    {
        DateTime CreationDate { get; }

        public Guid Id { get; set; }
    }
}
