using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Services
{
    public class PasswordService
    {
        public PasswordService() 
        {

        }

        public static byte[] GenerateSalt(int size = 32)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string GenerateSaltBase64(int size = 32)
        {
            byte[] salt = GenerateSalt(size);
            return Convert.ToBase64String(salt);
        }

        // Hash password with PBKDF2 using a given salt
        public static string HashPassword(string password, byte[] salt, int iterations = 10000, int hashSize = 32)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(hashSize);
                return Convert.ToBase64String(hash); // Return hashed password as Base64 string
            }
        }

        // Verify if the password matches the hash by hashing the input and comparing
        public static bool VerifyPassword(string password, string hashedPassword, string salt, int iterations = 10000, int hashSize = 32)
        {
            byte[] saltBytes = Convert.FromBase64String(salt); // Convert Base64 salt back to byte array
            string hashedInputPassword = HashPassword(password, saltBytes, iterations, hashSize);
            return hashedInputPassword == hashedPassword;
        }
    }
}
