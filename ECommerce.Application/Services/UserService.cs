using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            // Fetch user by username
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) return null; // User not found

            var verificationResult = IsValidPassword(password, user.Password);

            if (verificationResult == false)
            {
                return null; // Authentication failed
            }

            return user; // Authentication successful
        }

        private bool IsValidPassword(string loginPassword, string savedPassword)
        {
            bool result = false;
            if (loginPassword == savedPassword)
                result = true;

            return result;
        }

    }
}
