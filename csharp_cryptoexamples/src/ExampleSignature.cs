using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace com.cryptoexamples.csharp
{
    public class ExampleSigning
    {
        public static StringWriter LOGGER = new StringWriter();

        public static void Main(string[] args)
        {
            DemonstrateSigning("Text that should be signed to prevent unknown tampering with its content.");
        }

        public static Boolean DemonstrateSigning(String plainText)
        {
            try
            {
                // Create a UnicodeEncoder to convert between byte array and string.
                UTF8Encoding ByteConverter = new UTF8Encoding();

                // Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] originalData = ByteConverter.GetBytes(plainText);
                byte[] signedData;

                // Create a new instance of the RSACryptoServiceProvider class 
                // and automatically create a new key-pair.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                // Hash and sign the data.
                signedData = RSAalg.SignData(originalData, new SHA1CryptoServiceProvider());

                // Verify the data and display the result to the console
                if (RSAalg.VerifyData(originalData, new SHA1CryptoServiceProvider(), signedData))
                {
                    Console.Write("The data was verified.");
                    return true;
                }
            }
            catch (CryptographicException e)
            {
                Console.Write(e.Message);
            }
            return false;
        }
    }
}
