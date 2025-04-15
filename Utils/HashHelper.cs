using System.Security.Cryptography;
using System.Text;

namespace ManagementBook.Utils
{
    public static class Sha256Helper
    {
        public static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}