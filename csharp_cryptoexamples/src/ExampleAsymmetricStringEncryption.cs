using Serilog;
using System;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for asymmetric encryption and decryption of a string in one method.
    /// <para>- Generation of public and private RSA 4096 bit keypair</para>
    /// <para>- BASE64 encoding as representation for the byte-arrays</para>
    /// <para>- UTF-8 encoding of Strings</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/RSA_(cryptosystem)" />
    /// </summary>
    public static class ExampleAsymetricStringEncryption
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateAsymetricStringEncryption("Text that is going to be sent over an insecure channel and must be encrypted at all costs!");
        }

        public static string DemonstrateAsymetricStringEncryption(string plainText)
        {
            byte[] dataForEncryption = System.Text.Encoding.UTF8.GetBytes(plainText);
            //Create a new key pair with keysize 4096.
            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider
            {
                KeySize = 4096
            };

            #region - Encrypt -
            
            byte[] encryptedData = rSACryptoServiceProvider.Encrypt(dataForEncryption, false);
            string encryptedString = Convert.ToBase64String(encryptedData);

            #endregion

            #region - Decrypt -
            
            byte[] decryptedData = Convert.FromBase64String(encryptedString);
            string decryptedString = System.Text.Encoding.UTF8.GetString(rSACryptoServiceProvider.Decrypt(decryptedData, false));

            Log.Information("Decrypted and original plain text are the same: {0}", decryptedString.Equals(plainText));

            #endregion

            return decryptedString;
        }
    }
}
