using Serilog;
using System;
using System.Security.Cryptography;
using System.Text;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for hashing of a string in one method.
    /// <para>- SHA-512</para>
    /// <para>- BASE64 encoding as representation for the byte-arrays</para>
    /// <para>- UTF-8 encoding of String</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/SHA-2" />
    /// </summary>
    public static class ExampleHashing
    {
        public static string DemonstrateHashing(string plainText)
        {
            HashAlgorithm hashAlgorithm = SHA512.Create();
            string hashString = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(plainText)));
            Log.Information("The hashed value is: {0}", hashString);
            return hashString;
        }

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateHashing("Text that should be authenticated by comparing the hash of it!");
        }
    }
}