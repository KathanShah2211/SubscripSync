using Microsoft.EntityFrameworkCore;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using SubscripSync.Infrastructure.Data;
using System.Threading.Tasks;

namespace SubscripSync.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SubscripSyncDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
