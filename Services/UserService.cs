using Microsoft.EntityFrameworkCore;
using Test2Server.Data;
using Test2Server.Data.Entities;

namespace Test2Server.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            await _dbContext.users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity> GetOneUserByEmail(string email)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new InvalidOperationException($"User with email '{email}' not found.");
            }

            return user;
        }
    }

}