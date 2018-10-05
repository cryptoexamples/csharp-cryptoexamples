using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace com.cryptoexamples.csharp
{
    public static class ExampleSignature
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateSigning("Text that should be signed to prevent unknown tampering with its content.");
        }

        public static Boolean DemonstrateSigning(String plainText)
        {
            try
            {
                // Create a UnicodeEncoder to convert between byte array and string.
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                // Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] originalData = uTF8Encoding.GetBytes(plainText);
                // Create a new instance of the RSACryptoServiceProvider class 
                // and automatically create a new key-pair.
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                // Hash and sign the data. Pass a new instance of SHA256CryptoServiceProvider
                // to specify the use of SHA256 for hashing.
                // Hash and sign the data.
                byte[] signedData = rSACryptoServiceProvider.SignData(originalData, new SHA256CryptoServiceProvider());
                // Verify the data.
                if (rSACryptoServiceProvider.VerifyData(originalData, new SHA256CryptoServiceProvider(), signedData))
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
