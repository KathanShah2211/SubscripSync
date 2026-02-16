using System.Threading.Tasks;
using SubscripSync.Domain.Entities;

namespace SubscripSync.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
