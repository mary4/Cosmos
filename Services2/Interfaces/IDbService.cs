using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDbService
    {
        CosmosClient CreateClient(string endpointUrl, string primaryKey);
        Task<Database> CreateDatabase(CosmosClient client, string dbName);
        Task<Container> CreateContainer(CosmosClient client, string databaseId, string containerId, string key);
        Task<Container> CreateContainer(Database database, string containerId, string key);
    }
}

