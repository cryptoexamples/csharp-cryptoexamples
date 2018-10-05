using Serilog;
using System;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    public static class ExampleAsymetricStringEncryption
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateAsymetricStringEncryption("Text that is going to be sent over an insecure channel and must be encrypted at all costs!");
        }

        public static String DemonstrateAsymetricStringEncryption(String plainText)
        {
            byte[] dataForEncryption = System.Text.Encoding.UTF8.GetBytes(plainText);

            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider
            {
                KeySize = 512
            };

            //----------------------------Encrypt----------------------------
            byte[] encryptedData = rSACryptoServiceProvider.Encrypt(dataForEncryption, false);
            String encryptedString = Convert.ToBase64String(encryptedData);

            //----------------------------Decrypt----------------------------
            byte[] decryptedData = Convert.FromBase64String(encryptedString);
            String decryptedString = System.Text.Encoding.UTF8.GetString(rSACryptoServiceProvider.Decrypt(decryptedData, false));

            Log.Information("Decrypted and original plain text are the same: {0}", decryptedString.Equals(plainText));

            return decryptedString;
        }
    }
}
