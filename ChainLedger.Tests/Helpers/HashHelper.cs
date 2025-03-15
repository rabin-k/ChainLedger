using ChainLedger.Models;
using System.Security.Cryptography;
using System.Text;

namespace ChainLedger.Tests.Helpers
{
    public class HashHelper
    {
        // Helper method to simulate the hash calculation
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
