﻿using Serilog;
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
            //Contains the binary representation of the string that should be encrypted.
            byte[] dataForEncryption = System.Text.Encoding.UTF8.GetBytes(plainText);
            //Create a new key pair with keysize of 4096.
            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider
            {
                KeySize = 4096
            };

            //----------------------------Encrypt----------------------------

            //Encrypt the binary string with the RSA-Provider.
            byte[] encryptedData = rSACryptoServiceProvider.Encrypt(dataForEncryption, false);
            //Convert the encrypted binary to a string.
            string encryptedString = Convert.ToBase64String(encryptedData);

            //----------------------------Decrypt----------------------------

            //Convert the encrypted string to it's binary representation.
            byte[] decryptedData = Convert.FromBase64String(encryptedString);
            //Decrypt the data with the RSA-Provider and convert the resulting byte array to an UTF-8 string.
            string decryptedString = System.Text.Encoding.UTF8.GetString(rSACryptoServiceProvider.Decrypt(decryptedData, false));

            Log.Information("Decrypted and original plain text are the same: {0}", decryptedString.Equals(plainText));

            return decryptedString;
        }
    }
}
