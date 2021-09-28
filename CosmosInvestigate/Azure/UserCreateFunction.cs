using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Interfaces;
using Azure.Models;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using User = Services.Models.User;

namespace Azure
{
    public class UserCreateFunction
    {
        private IMapper _mapper;
        private IUserService _userService;
        private IDbService _dbService;

        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _userCollection;

        public UserCreateFunction(IUserService userService, IMapper mapper, IDbService dbService)
        {
            _mapper = mapper;
            _userService = userService;
            _dbService = dbService;
        }

        [FunctionName("create")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserDto data = JsonConvert.DeserializeObject<UserDto>(requestBody);

                await InitCosmosClient();

                var user = _mapper.Map<User>(data);
                await _userService.CreateUser(_userCollection, user);

                return new OkObjectResult(_mapper.Map<UserDto>(user));
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
