using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Security
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }  // Secret key for JWT signing
        public string Issuer { get; set; }     // The issuer (usually the API's name or URL)
        public string Audience { get; set; }   // The audience (who the token is for)
        public int ExpirationInMinutes { get; set; }  // Token expiration time (in minutes)
    }
}
