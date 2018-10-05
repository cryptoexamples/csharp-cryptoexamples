using Serilog;
using System;
using System.IO;
using System.Security.Cryptography;

namespace com.cryptoexamples.csharp
{
    public static class ExampleStringEncryptionPasswordBased
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateStringEncryptionPasswordBased("Text that is going to be sent over an insecure channel and must be encrypted at all costs!", "SuperSafe");
        }

        public static String DemonstrateStringEncryptionPasswordBased(String plainText, String password)
        {
            String decryptedCipherText = "";
            try
            {
                // Create a new instance of the AesManaged class. This generates a new key and initialization vector (IV).
                using (AesManaged aesManaged = new AesManaged())
                {
                    //----------------------------Encrypt----------------------------
                    //If no password is provided througth the user, use the generated key from the AesManaged class.
                    if (!string.IsNullOrEmpty(password))
                    {
                        aesManaged.KeySize = 256;
                        //Generating random salt
                        byte[] salt = new byte[32];
                        // Fille the buffer with the generated data
                        new RNGCryptoServiceProvider().GetBytes(salt);
                        //Derive the key from the password, salt and an iteration of 50000 with PBKDF2.
                        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 50000);
                        //Bring the password in the correct size
                        byte[] keyBytes = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8);
                        aesManaged.Key = keyBytes;
                    }
                    // Contains the encrypted string as bytes.
                    byte[] cipherTextBytes;
                    // Create a encrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)))
                        {
                            //Write all data to the stream.
                            streamWriter.Write(plainText);
                        }
                        cipherTextBytes = memoryStream.ToArray();
                    }

                    //----------------------------Decrypt----------------------------
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV);
                    // Read the decrypted bytes from the decrypting stream and place them in a string.
                    decryptedCipherText = new StreamReader(new CryptoStream(new MemoryStream(cipherTextBytes), decryptor, CryptoStreamMode.Read)).ReadToEnd();
                    Log.Information("Decrypted and original plain text are the same: {0}", plainText.Equals(decryptedCipherText));
                }
            }
            catch (CryptographicException e) { Log.Error("Error: {0}", e.Message); }
            catch (NullReferenceException e) { Log.Error("Error: {0}", e.Message); }
            return decryptedCipherText;
        }
    }
}
