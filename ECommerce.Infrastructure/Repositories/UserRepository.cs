using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceDbContext _dbContext;
        public UserRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var result = await _dbContext.User.FirstOrDefaultAsync(u => u.Username == username);
            return result;
        }
    }
}
