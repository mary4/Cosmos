using Microsoft.Azure.Cosmos;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DbService : IDbService
    {
        public CosmosClient CreateClient(string endpointUrl, string primaryKey)
        {
            var options = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            var client = new CosmosClient(endpointUrl, primaryKey, options);
            return client;
        }

        public async Task<Container> CreateContainer(CosmosClient client, string databaseId, string containerId, string key)
        {
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(databaseId);
            var container = await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerId, key);

            return container;
        }

        public async Task<Container> CreateContainer(Database database, string containerId, string key)
        {
            var container = await database.CreateContainerIfNotExistsAsync(containerId, key);
            return container;
        }

        public async Task<Database> CreateDatabase(CosmosClient client, string databaseId)
        {
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(databaseId);
            return databaseResponse.Database;
        }
    }
}
