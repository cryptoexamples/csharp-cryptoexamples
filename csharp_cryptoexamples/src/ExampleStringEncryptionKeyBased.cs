using Serilog;
using System;
using System.IO;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for encryption and decryption of a string in one method.
    /// <para>- AES-256</para>
    /// <para>- BASE64 encoding as representation for the byte-arrays</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/Advanced_Encryption_Standard" />
    /// </summary>
    public static class ExampleStringEncryptionKeyBased
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateStringEncryptionKeyBased("Text that is going to be sent over an insecure channel and must be encrypted at all costs!");
        }

        public static string DemonstrateStringEncryptionKeyBased(string plainText)
        {
            string decryptedCipherText = "";
            try
            {
                //Generate a new key and initialization vector.
                using (AesManaged aesManaged = new AesManaged())
                {
                    //----------------------------Encrypt----------------------------
                    //Spezify the keysize.
                    aesManaged.KeySize = 256;
                    //Contains the encrypted string as bytes representataion.
                    byte[] cipherTextBytes;
                    //Contains the ciphertext.
                    string cipherText;
                    //Create an encrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)))
                        {
                            //Write all data to the stream.
                            streamWriter.Write(plainText);
                        }
                        cipherTextBytes = memoryStream.ToArray();
                        //Convert the byte representation of the ciphertext to a base64 string.
                        cipherText = Convert.ToBase64String(cipherTextBytes);
                    }

                    //----------------------------Decrypt----------------------------

                    //Convert the cipher string to it's base64 byte representation.
                    byte[] decryptedCipherTextBytes = Convert.FromBase64String(cipherText);
                    //Create a decrytor with the same key and iv as the encrypter to decrypt the stream.
                    ICryptoTransform decryptor = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV);
                    //Read the streams and perform the encryption on it.
                    decryptedCipherText = new StreamReader(new CryptoStream(new MemoryStream(decryptedCipherTextBytes), decryptor, CryptoStreamMode.Read)).ReadToEnd();
                }
                
                Log.Information("Decrypted and original plain text are the same: {0}", plainText.Equals(decryptedCipherText));
            }
            catch (CryptographicException e)
            {
                Log.Error("Error: {0}", e.Message);
            }

            return decryptedCipherText;
        }
    }
}
