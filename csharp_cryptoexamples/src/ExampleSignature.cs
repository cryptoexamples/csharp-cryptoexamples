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
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateSigning("Text that should be signed to prevent unknown tampering with its content.");
        }
        
        public static bool DemonstrateSigning(string plainText)
        {
            try
            {
                //----------------------------Signing---------------------------------

                //Convert the plaintext to there utf-8 byte representation.
                byte[] originalData = Encoding.UTF8.GetBytes(plainText);
                //Generate a new key-pair.
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider
                {
                    KeySize = 4096
                };
                //Hash and sign the data with SHA-512.
                byte[] signedData = rSACryptoServiceProvider.SignData(originalData, new SHA512CryptoServiceProvider());
                //Convert the signed data to a representative string.
                string signature = Convert.ToBase64String(signedData);


                //----------------------------Verification----------------------------

                //Convert the signature to the base64 byte representation.
                signedData = Convert.FromBase64String(signature);
                //Verify the data.
                if (rSACryptoServiceProvider.VerifyData(originalData, new SHA512CryptoServiceProvider(), signedData))
                {
                    Log.Information("The data is verified.");
                    return true;
                }
                else { Log.Information("The data is not verified."); }
            }
            catch (CryptographicException e)
            {
                Log.Error("Error: {0}", e.Message);
            }
            return false;
        }
    }
}
