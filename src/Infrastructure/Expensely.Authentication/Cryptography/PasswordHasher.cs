using System;
using System.Security.Cryptography;
using Expensely.Authentication.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Expensely.Authentication.Cryptography
{
    /// <summary>
    /// Represents a password hasher, used for hashing passwords and verifying hashed passwords.
    /// </summary>
    internal sealed class PasswordHasher : IPasswordHasher, IDisposable
    {
        private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
        private const int IterationCount = 10000;
        private const int NumberOfBytesRequested = 256 / 8;
        private const int SaltSize = 128 / 8;
        private readonly RandomNumberGenerator _rng;

        public PasswordHasher()
        {
            _rng = new RNGCryptoServiceProvider();
        }

        public string HashPassword(string password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            string hashedPassword = Convert.ToBase64String(HashPasswordInternal(password));

            return hashedPassword;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failure;
            }

            bool verified = VerifyHashedPasswordInternal(decodedHashedPassword, providedPassword);

            return verified ? PasswordVerificationResult.Success : PasswordVerificationResult.Failure;
        }

        public void Dispose()
        {
            _rng.Dispose();
        }

        /// <summary>
        /// Returns the bytes of the hash for the specified password.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The bytes of the hash for the specified password.</returns>
        private byte[] HashPasswordInternal(string password)
        {
            byte[] salt = GetSalt();

            byte[] subKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, NumberOfBytesRequested);

            byte[] outputBytes = new byte[salt.Length + subKey.Length];

            Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);

            Buffer.BlockCopy(subKey, 0, outputBytes, salt.Length, subKey.Length);

            return outputBytes;
        }

        /// <summary>
        /// Gets a randomly generated salt.
        /// </summary>
        /// <returns>The randomly generated salt.</returns>
        private byte[] GetSalt()
        {
            byte[] salt = new byte[SaltSize];

            _rng.GetBytes(salt);

            return salt;
        }

        /// <summary>
        /// Verifies the bytes of the hashed password with the specified password.
        /// </summary>
        /// <param name="hashedPassword">The bytes of the hashed password.</param>
        /// <param name="password">The password to verify with.</param>
        /// <returns>True if the hashes match, otherwise false.</returns>
        private static bool VerifyHashedPasswordInternal(byte[] hashedPassword, string password)
        {
            try
            {
                byte[] salt = new byte[SaltSize];

                Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

                int subKeyLength = hashedPassword.Length - salt.Length;

                if (subKeyLength < SaltSize)
                {
                    return false;
                }

                byte[] expectedSubKey = new byte[subKeyLength];

                Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubKey, 0, expectedSubKey.Length);

                byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, subKeyLength);

                return ByteArraysEqual(actualSubKey, expectedSubKey);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the specified byte arrays are equal, otherwise false.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if the arrays are equal, otherwise false.</returns>
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;

            for (int i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }
    }
}
