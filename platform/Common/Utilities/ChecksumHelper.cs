using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class ChecksumHelper
    {
        /// <summary>
        ///     Join input and key to request MD5Hash
        /// </summary>
        /// <param name="input">String to generate checksum for.</param>
        /// <returns>MD5 Hash</returns>
        public static string BuildChecksum(string input)
        {
            return CryptographyExtensions.GetHashMd5(input).ToLowerInvariant();
        }
    }

    public static class CryptographyExtensions
    {
        /// <summary>
        /// 	Calculates the MD5 hash for the given string.
        /// </summary>
        /// <returns>A 32 char long MD5 hash.</returns>
        public static string GetHashMd5(this string input)
        {
            return ComputeHash(input, new MD5CryptoServiceProvider());
        }

        /// <summary>
        /// 	Calculates the SHA-1 hash for the given string.
        /// </summary>
        /// <returns>A 40 char long SHA-1 hash.</returns>
        public static string GetHashSha1(this string input)
        {
            return ComputeHash(input, new SHA1Managed());
        }

        /// <summary>
        /// 	Calculates the SHA-256 hash for the given string.
        /// </summary>
        /// <returns>A 64 char long SHA-256 hash.</returns>
        public static string GetHashSha256(this string input)
        {
            return ComputeHash(input, new SHA256Managed());
        }

        /// <summary>
        /// 	Calculates the SHA-384 hash for the given string.
        /// </summary>
        /// <returns>A 96 char long SHA-384 hash.</returns>
        public static string GetHashSha384(this string input)
        {
            return ComputeHash(input, new SHA384Managed());
        }

        /// <summary>
        /// 	Calculates the SHA-512 hash for the given string.
        /// </summary>
        /// <returns>A 128 char long SHA-512 hash.</returns>
        public static string GetHashSha512(this string input)
        {
            return ComputeHash(input, new SHA512Managed());
        }

        public static string ComputeHash(string input, HashAlgorithm hashProvider)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (hashProvider == null)
            {
                throw new ArgumentNullException("hashProvider");
            }

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = hashProvider.ComputeHash(inputBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            return hash;
        }

        public static string DecryptStringFromBase64(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string EnryptStringToBase64(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}