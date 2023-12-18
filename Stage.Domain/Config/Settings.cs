using Microsoft.Extensions.Configuration;

namespace Stage.Domain.Config
{
    public struct Database
    {
        public string ConnectionString { get; set; }
    }

    public static class Settings
    {
        public static Database Database { get; private set; }

        public static void Configure(IConfigurationRoot configuration)
        {
            Database = new Database()
            {
                ConnectionString = configuration.GetSection("Database:ConnectionString").Value ?? ""
            };
        }
    }
}
