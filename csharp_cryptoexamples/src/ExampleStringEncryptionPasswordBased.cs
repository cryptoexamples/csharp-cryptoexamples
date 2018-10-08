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
    /// <para>- Random salt generation</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/Advanced_Encryption_Standard" />
    /// </summary>
    public static class ExampleStringEncryptionPasswordBased
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateStringEncryptionPasswordBased("Text that is going to be sent over an insecure channel and must be encrypted at all costs!", "SuperSafe");
        }

        public static string DemonstrateStringEncryptionPasswordBased(string plainText, string password)
        {
            string decryptedCipherText = "";
            try
            {
                //Generate a new key and initialization vector.
                using (AesManaged aesManaged = new AesManaged())
                {
                    //If the password from the user can't be empty or null. Instead this would be the same as 'StringEncryptionKeyBased'.
                    if (!string.IsNullOrEmpty(password))
                    {
                        //----------------------------Encrypt----------------------------

                        //Spezify the keysize.
                        aesManaged.KeySize = 256;
                        //Generating random salt.
                        byte[] salt = new byte[32];
                        //Fill the salt with random generated data.
                        new RNGCryptoServiceProvider().GetBytes(salt);
                        //Derive the key from the password, salt and an iteration of 50000 with PBKDF2.
                        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 50000);
                        //Set the password in the correct size.
                        byte[] keyBytes = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8);
                        aesManaged.Key = keyBytes;
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
                        Log.Information("Decrypted and original plain text are the same: {0}", plainText.Equals(decryptedCipherText));
                    }
                    else { Log.Error("Error: {0}", "Password can't be empty or null!"); }
                }
            }
            catch (CryptographicException e) { Log.Error("Error: {0}", e.Message); }
            catch (NullReferenceException e) { Log.Error("Error: {0}", e.Message); }
            return decryptedCipherText;
        }
    }
}
