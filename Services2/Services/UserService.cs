using Microsoft.Azure.Cosmos;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using User = Services.Models.User;

namespace Services.Services
{
    public class UserService : IUserService
    {
        public async Task CreateUser(Container container, User user)
        {
            try
            {
                user.Id = user.Id == null ? Guid.NewGuid() : user.Id;
                ItemResponse<User> response = await container.CreateItemAsync<User>(user, new PartitionKey(user.Id.ToString()));
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Item in database with id: {0} already exists\n", user.Id);
            }
            catch (CosmosException cosmosEx)
            {
                Console.WriteLine($"Cosmos exception: {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task DeleteUser(Container container, Guid id)
        {
            var response = await container.DeleteItemAsync<User>(id.ToString(), new PartitionKey(id.ToString()));
            Console.WriteLine("Operation consumed {0} RUs.\n", response.RequestCharge);
        }

        public async Task<User> GetUser(Container container, Guid id)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<User> queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);
            List<User> users = new List<User>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<User> response = await queryResultSetIterator.ReadNextAsync();
                foreach (User user in response)
                {
                    users.Add(user);
                }
                Console.WriteLine("Operation consumed {0} RUs.\n", response.RequestCharge);
            }
            return users.FirstOrDefault();
        }

        public async Task<User> UpdateUser(Container container, Guid id, User user)
        {

            if (id.Equals(user.Id))
            {
                var response = await container.UpsertItemAsync<User>(user, new PartitionKey(id.ToString()));
                Console.WriteLine("Operation consumed {0} RUs.\n", response.RequestCharge);

                return response.Resource;
            }

            throw new UserException("Given id does not conside with user id");
        }
    }
}
