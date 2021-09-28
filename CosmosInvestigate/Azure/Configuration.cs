using System;

namespace Azure
{
    public static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.ConnectionString");
            }
        }

        public static string Endpoint
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.Endpoint");
            }
        }

        public static string PrimaryKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.PrimaryKey");
            }
        }

        public static string DatabaseName
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.DatabaseName");
            }
        }
        public static string ContainerName
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.ContainerName");
            }
        }

        public static string UserContainerKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("Azure.CosmosDB.Container.Key.Users");
            }
        }
    }
}
