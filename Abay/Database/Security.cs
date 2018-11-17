using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class Security
    {
        public static string CreateToken(int length)
        {
            return BuildSecureToken(length);
        }

        public static string CreateHashedPassword(string password, out string retSalt)
        {
            string hashedPassword = GenerateHashedPassword(password, out string salt);
            retSalt = salt;
            return hashedPassword;
        }

        public static string CheckPassword(string password, string salt)
        {
            byte[] saltArray = Convert.FromBase64String(salt);
            return HashPassword(password, saltArray);
        }

        // Create Login token 
        private static string BuildSecureToken(int length)
        {
            var buffer = new byte[length];
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        // Create random Salt and generate the hashed password
        private static string GenerateHashedPassword(string password, out string salt)
        {
            byte[] saltArray = new byte[16];

            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider()) 
            {
                rngCryptoServiceProvider.GetBytes(saltArray);
            }
            salt = Convert.ToBase64String(saltArray);

            return HashPassword(password, saltArray);
        }

        // Hash password + salt togehter 
        private static string HashPassword(string password, byte[] salt)
        {
            byte[] hashedArray = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(36);
            return Convert.ToBase64String(hashedArray);
        }
    }
}
