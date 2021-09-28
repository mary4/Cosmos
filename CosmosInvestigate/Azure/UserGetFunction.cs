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
using Azure.Models;

namespace Azure
{
    public class UserGetFunction
    {
        private IMapper _mapper;
        private IUserService _userService;
        private IDbService _dbService;

        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _userCollection;

        public UserGetFunction(IUserService userService, IMapper mapper, IDbService dbService)
        {
            _mapper = mapper;
            _userService = userService;
            _dbService = dbService;
        }

        [FunctionName("read")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                await InitCosmosClient();

                var user = await _userService.GetUser(_userCollection, id);
                var userDto = _mapper.Map<UserDto>(user);

                return new OkObjectResult(userDto);
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
