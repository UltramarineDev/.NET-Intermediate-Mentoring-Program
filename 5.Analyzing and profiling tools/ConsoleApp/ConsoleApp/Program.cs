using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string text = "testtesttesttesttesttest";
            var salt = Encoding.ASCII.GetBytes("test salt test salt");

            GeneratePasswordHashUsingSalt(text, salt);
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            const int iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate, HashAlgorithmName.SHA256);

            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }
}
