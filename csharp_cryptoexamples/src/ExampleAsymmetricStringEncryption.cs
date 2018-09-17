using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace com.cryptoexamples.csharp
{
    public class ExampleAsymetricStringEncryption
    {
        public static ILogger logger = new LoggerFactory().CreateLogger<ExampleAsymetricStringEncryption>();

        public static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddProvider(new ConsoleLoggerProvider(
                (text, logLevel) => logLevel >= LogLevel.Verbose, true));
            logger.LogInformation("This is a test of the emergency broadcast system.");
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
