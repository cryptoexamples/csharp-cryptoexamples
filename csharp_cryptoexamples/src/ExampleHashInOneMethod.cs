using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace com.cryptoexamples.csharp
{
    public class ExampleHashing
    {
        public static void Main(string[] args)
        {
            DemonstrateHashing("Text that should be authenticated by comparing the hash of it!");
        }

        public static String DemonstrateHashing(String plainText)
        {
            HashAlgorithm algorithm = SHA256.Create();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in algorithm.ComputeHash(Encoding.UTF8.GetBytes(plainText)))
                stringBuilder.Append(b.ToString("X2")); //"X2" formats the string as two uppercase hexadecimal characters.
            
            return stringBuilder.ToString();
        }
    }
}