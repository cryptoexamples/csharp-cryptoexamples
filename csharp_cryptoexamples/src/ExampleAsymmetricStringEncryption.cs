using System;
using System.IO;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    public class ExampleAsymetricStringEncryption
    {
        public static StringWriter LOGGER = new StringWriter();

        public static void Main(string[] args)
        {
            DemonstrateAsymetricStringEncryption("Text that is going to be sent over an insecure channel and must be encrypted at all costs!");
        }

        public static String DemonstrateAsymetricStringEncryption(String plainText)
        {
            byte[] dataToEncrypt = System.Text.Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData;
            String decryptedString;

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Encrypt
            encryptedData = RSA.Encrypt(dataToEncrypt, false);
            //Decrypt
            decryptedString = System.Text.Encoding.UTF8.GetString(RSA.Decrypt(encryptedData, false));

            Console.Write("Decrypted and original plain text are the same: {0}", decryptedString.Equals(plainText));

            return decryptedString;
        }
    }
}
