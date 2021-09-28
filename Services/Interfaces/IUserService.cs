using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using User = Services.Models.User;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(Container container, User user);
        Task<User> UpdateUser(Container container, Guid id, User user);
        Task DeleteUser(Container container, Guid id);
        Task<User> GetUser(Container container, Guid id);
    }
}
