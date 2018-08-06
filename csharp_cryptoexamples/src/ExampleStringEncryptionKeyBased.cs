using System;
using System.IO;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    public class ExampleStringEncryptionKeyBasedInOneMethod
    {
        public static StringWriter LOGGER = new StringWriter();

        public static void Main(string[] args)
        {
            DemonstrateStringEncryptionKeyBased("Text that is going to be sent over an insecure channel and must be encrypted at all costs!");
        }

        public static String DemonstrateStringEncryptionKeyBased(String plainText)
        {
            String decryptedCipherText = "";
            try
            {
                // Create a new instance of the AesManaged class. This generates a new key and initialization vector (IV).
                using (AesManaged myAes = new AesManaged())
                {
                    //----------------------------Encrypt----------------------------

                    // Contains the encrypted string as bytes.
                    byte[] cipherTextBytes;

                    // Create a encrytor to perform the stream transform.
                    ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        cipherTextBytes = msEncrypt.ToArray();
                    }

                    //----------------------------Decrypt----------------------------

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);

                    // Read the decrypted bytes from the decrypting stream and place them in a string.
                    decryptedCipherText = new StreamReader(new CryptoStream(new MemoryStream(cipherTextBytes), decryptor, CryptoStreamMode.Read)).ReadToEnd();
                }

                //Display the original data and the decrypted data.
                Console.Write("They are the same: {0}", plainText.Equals(decryptedCipherText));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            return decryptedCipherText;
        }
    }
}
