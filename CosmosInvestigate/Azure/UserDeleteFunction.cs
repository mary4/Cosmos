using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using AutoMapper;
using Microsoft.Azure.Cosmos;

namespace Azure
{
    public class UserDeleteFunction
    {
        private IMapper _mapper;
        private IUserService _userService;
        private IDbService _dbService;

        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _userCollection;

        public UserDeleteFunction(IUserService userService, IMapper mapper, IDbService dbService)
        {
            _mapper = mapper;
            _userService = userService;
            _dbService = dbService;
        }

        [FunctionName("delete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "user/{id}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                await InitCosmosClient();
                await _userService.DeleteUser(_userCollection, id);

                return new OkObjectResult(id);
            }
            finally
            {
                log.LogInformation("C# HTTP trigger function finished.");
            }
        }

        private async Task InitCosmosClient()
        {
            _cosmosClient = _cosmosClient ?? _dbService.CreateClient(Configuration.Endpoint, Configuration.PrimaryKey);
            _database = _database ?? await _dbService.CreateDatabase(_cosmosClient, Configuration.DatabaseName);
            _userCollection = _userCollection ?? await _dbService.CreateContainer(_database, Configuration.ContainerName, Configuration.UserContainerKey);
        }
    }
}
