using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for cryptographic signing of a string in one method.
    /// <para>- Generation of public and private RSA 4096 bit keypair</para>
    /// <para>- SHA-512 with RSA</para>
    /// <para>- UTF-8 encoding of Strings</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/RSA_(cryptosystem)" />
    /// </summary>
    public static class ExampleSignature
    {
        public static bool DemonstrateSigning(string plainText)
        {
            try
            {
                #region SIGNING
                byte[] originalData = Encoding.UTF8.GetBytes(plainText);
                //Generate a new key-pair.
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider
                {
                    KeySize = 4096
                };
                byte[] signedData = rSACryptoServiceProvider.SignData(originalData, new SHA512CryptoServiceProvider());
                string signature = Convert.ToBase64String(signedData);
                #endregion

                #region VERIFIACTION
                signedData = Convert.FromBase64String(signature);
                Log.Error("Signature is correct: {0}", rSACryptoServiceProvider.VerifyData(originalData, new SHA512CryptoServiceProvider(), signedData));
                #endregion
            }
            catch (CryptographicException e)
            {
                Log.Error("Error: {0}", e.Message);
            }
            return false;
        }

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateSigning("Text that should be signed to prevent unknown tampering with its content.");
        }
        
    }
}
