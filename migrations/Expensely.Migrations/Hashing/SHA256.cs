using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Expensely.Migrations.Hashing
{
    /// <summary>
    /// Represents the SHA256 hash function.
    /// </summary>
    public static class SHA256
    {
        private const string CryptoAlgorithmName = nameof(SHA256);

        /// <summary>
        /// Computes the hash for the given input using the SHA256 hash function.
        /// </summary>
        /// <param name="input">The input to be hashed.</param>
        /// <returns>The computed hash value as a string.</returns>
        public static string ComputeHash(string input)
        {
            byte[] utf8EncodedInput = new UTF8Encoding().GetBytes(input);

            using var sha256 = (HashAlgorithm)CryptoConfig.CreateFromName(CryptoAlgorithmName);

            byte[] hash = sha256.ComputeHash(utf8EncodedInput);

            var stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            string output = stringBuilder.ToString();

            return output;
        }
    }
}
