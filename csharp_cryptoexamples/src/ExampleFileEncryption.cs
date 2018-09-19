using System;
using System.Security.Cryptography;
using System.IO;
using Serilog;

namespace com.cryptoexamples.csharp
{
    public static class ExampleFileEncryption
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information(DemonstrateFileEncryption("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", "ThePasswordToDecryptAndEncryptTheFile"));
        }

        public static String DemonstrateFileEncryption(String plainText, string password)
        {
            String inputFile = @"encryptedFile.enc";

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

            //Encryption

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = CipherMode.CBC;
            //create output file name
            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Create))
            {
                // write salt to the begining of the output file, so in this case can be random every time
                fsCrypt.Write(salt, 0, salt.Length);
                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    try
                    {
                        cs.Write(plaintextBytes);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                    }
                }
            }

            //Decryption

            String decryptString = "";

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                fsCrypt.Read(salt, 0, salt.Length);

                using (CryptoStream cs2 = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader fsOut = new StreamReader(cs2))
                    {
                        try
                        {
                            decryptString = fsOut.ReadToEnd();
                        }
                        catch (CryptographicException e)
                        {
                            Log.Error(e.Message);
                        }
                    }
                }
            }

            return decryptString;
        }
    }
}
