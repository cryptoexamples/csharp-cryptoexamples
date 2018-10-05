using Serilog;
using System;
using System.Security.Cryptography;
using System.Text;

namespace com.cryptoexamples.csharp
{
    public static class ExampleHashing
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateHashing("Text that should be authenticated by comparing the hash of it!");
        }

        public static String DemonstrateHashing(String plainText)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();
            String hashString = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(plainText)));
            Log.Information("The hashed value is: {0}", hashString);
            return hashString;
        }
    }
}