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
            // Fille the buffer with the generated data
            new RNGCryptoServiceProvider().GetBytes(salt);

            //----------------------------Encrypt----------------------------
            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            //Set AES symmetric encryption algorithm
            AesManaged aesManaged = new AesManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };
            //Derive bytes for the password with PBKDF2.
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aesManaged.Key = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8);
            aesManaged.IV = rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8);
            aesManaged.Mode = CipherMode.ECB; //TODO change CipherMode.
            //create output file name
            using (FileStream fileStream = new FileStream(inputFile, FileMode.Create))
            {
                // write salt to the begining of the output file, so in this case can be random every time
                fileStream.Write(salt, 0, salt.Length);
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aesManaged.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plaintextBytes);
                }
            }

            //----------------------------Decrypt----------------------------
            String decryptedString = "";

            using (FileStream fileStream = new FileStream(inputFile, FileMode.Open))
            {
                try
                {
                    fileStream.Read(salt, 0, salt.Length);
                    using (StreamReader streamReader = new StreamReader(new CryptoStream(fileStream, aesManaged.CreateDecryptor(), CryptoStreamMode.Read)))
                    {
                        decryptedString = streamReader.ReadToEnd();
                    }
                }
                catch (ArgumentException e)
                {
                    Log.Error("Error: {0}", e.Message);
                }
            }
            Log.Information("Decrypted file content and original plain text are the same: {0}", plainText.Equals(decryptedString));
            return decryptedString;
        }
    }
}
