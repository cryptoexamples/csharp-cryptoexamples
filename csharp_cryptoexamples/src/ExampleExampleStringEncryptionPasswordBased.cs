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
                using (AesManaged myAes = new AesManaged())
                {
                    //----------------------------Encrypt----------------------------
                    //If no password is provided througth the user, use the generated key from the AesManaged class.
                    if (password != null && password.Length != 0)
                    {
                        //Generating random salt
                        byte[] salt = new byte[32];
                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                // Fille the buffer with the generated data
                                rng.GetBytes(salt);
                            }
                        }
                        //Concat the password with the salt and get an new object
                        PasswordDeriveBytes passw = new PasswordDeriveBytes(password, salt);
                        //Bring the password in the correct size
                        byte[] keyBytes = passw.GetBytes(myAes.KeySize / 8);
                        myAes.Key = keyBytes;
                    }
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
                Log.Information("They are the same: {0}", plainText.Equals(decryptedCipherText));
            }
            catch (Exception e)
            {
                Log.Error("Error: {0}", e.Message);
            }

            return decryptedCipherText;
        }
    }
}
