using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL.Services.HashService
{
    public class HashService : IHashService
    {
        private const string Salt = "fKk+GkLvQBjAhe2VBDBnwg==";

        public string HashString(string source)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: source,
                salt: Convert.FromBase64String(Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));
        }
    }
}
