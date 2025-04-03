using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class User
    {
        public int Id { get;  set; }
        public string Username { get;  set; }
        public string Password { get;  set; }
        public string Email { get;  set; }
        public Role Role { get; set; } // Value Object

        // Constructor for User entity
        public User(string username, string password, string email, Role role)
        {
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }
    }

    // Value Object for Role (Admin, Customer)
    public enum Role
    {
        Admin = 0,
        Customer = 1
    }
}
