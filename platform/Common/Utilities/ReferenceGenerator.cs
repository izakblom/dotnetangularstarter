using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.SeedWork
{
    public static class ReferenceGenerator
    {
        public static int GenerateInt(int length = 10, string allowedChars = "123456789")
        {
            string reference = Generate(length, allowedChars);

            return Int32.Parse(reference);
        }
        //public static string Generate(int length = 10, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        //removed l L i I and 1 from the generated references for legibility
        //public static string Generate(int length = 10, string allowedChars = "abcdefghjkmnopqrstuvwxyzABCDEFGHJKMNOPQRSTUVWXYZ023456789")
        //removed o O 0 from the generated references again for legibility
        public static string Generate(int length = 10, string allowedChars = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            //using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            //{
            //    var result = new StringBuilder();
            //    var buf = new byte[128];
            //    while (result.Length < length)
            //    {
            //        rng.GetBytes(buf);
            //        for (var i = 0; i < buf.Length && result.Length < length; ++i)
            //        {
            //            // Divide the byte into allowedCharSet-sized groups. If the
            //            // random value falls into the last group and the last group is
            //            // too small to choose from the entire allowedCharSet, ignore
            //            // the value in order to avoid biasing the result.
            //            var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
            //            if (outOfRangeStart <= buf[i]) continue;
            //            result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
            //        }
            //    }
            //    return result.ToString();
            //}

            var result = new StringBuilder();
            var buf = new byte[128];

            var ran = System.Security.Cryptography.RandomNumberGenerator.Create();
            while (result.Length < length)
            {
                ran.GetBytes(buf);
                for (var i = 0; i < buf.Length && result.Length < length; ++i)
                {
                    // Divide the byte into allowedCharSet-sized groups. If the
                    // random value falls into the last group and the last group is
                    // too small to choose from the entire allowedCharSet, ignore
                    // the value in order to avoid biasing the result.
                    var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                    if (outOfRangeStart <= buf[i]) continue;
                    result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                }
            }
            return result.ToString();

        }
    }
}
