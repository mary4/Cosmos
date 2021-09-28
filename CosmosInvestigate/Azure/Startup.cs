using Azure;
using Azure.Mappings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Interfaces;
using Services.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Azure
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.TryAddScoped<IUserService, UserService>();
            builder.Services.TryAddScoped<IDbService, DbService>();
            builder.Services.AddAutoMapper(c => c.AddProfile<UserMapping>(), typeof(Startup));
        }
    }
}
