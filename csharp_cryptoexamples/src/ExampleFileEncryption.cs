using System;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;

namespace com.cryptoexamples.csharp
{
    public class ExampleFileEncryption
    {
        public static StringWriter LOGGER = new StringWriter();

        public static void Main(string[] args)
        {
            LOGGER.Write(DemonstrateFileEncryption("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", "ThePasswordToDecryptAndEncryptTheFile"));
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
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

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
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            //Decryption

            String decryptString = "";

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                fsCrypt.Read(salt, 0, salt.Length);

                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader fsOut = new StreamReader(cs))
                    {
                        try
                        {
                            decryptString = fsOut.ReadToEnd();
                        }
                        catch (CryptographicException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }

            return decryptString;
        }
    }
}
