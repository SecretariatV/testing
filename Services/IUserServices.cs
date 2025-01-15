using Test2Server.Data.Entities;

namespace Test2Server.Services
{
    public interface IUserService
    {
        Task<UserEntity> CreateUser(UserEntity user);
        Task<UserEntity> GetOneUserByEmail(string email);
    }
}