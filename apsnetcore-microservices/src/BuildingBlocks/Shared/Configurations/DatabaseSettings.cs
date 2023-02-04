namespace Shared.Configurations
{
    public class DatabaseSettings
    {
        public string DBProvider { get; set; } // Dùng khi muốn switch giữa các Database với nhau

        public string ConnectionString { get; set; }
    }
}
