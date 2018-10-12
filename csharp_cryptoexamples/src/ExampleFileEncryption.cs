using System;
using System.IO;
using System.Text;
using Serilog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for encryption and decryption of a file in one method.
    /// <para>- PKCS#5 Password-Based Cryptography Specification V2.0</para>
    /// <para>- Random salt generation</para>
    /// <para>- AES-256 authenticated encryption using GCM</para>
    /// <para>- BASE64-encoding as representation for the byte-arrays</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/Advanced_Encryption_Standard" />
    /// </summary>
    public static class ExampleFileEncryption
    {        
        public static string DemonstrateFileEncryption(string plainText, string password)
        {
            #region INITIALIZATION
            SecureRandom Random = new SecureRandom();
            byte[] dataForEncryption = Encoding.UTF8.GetBytes(plainText);
            Pkcs5S2ParametersGenerator pkcs5S2ParametersGenerator = new Pkcs5S2ParametersGenerator();
            byte[] salt = new byte[128 / 8];
            Random.NextBytes(salt);

            pkcs5S2ParametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt, 10000);

            KeyParameter key = (KeyParameter)pkcs5S2ParametersGenerator.GenerateDerivedMacParameters(256);
            
            byte[] nonce = new byte[128 / 8];
            Random.NextBytes(nonce);
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(new KeyParameter(key.GetKey()), 128, nonce, salt);
            gcmBlockCipher.Init(true, aeadParameters);

            #endregion
            #region ENCRYPTION
            //Generate ciphertext with authentication tag.
            byte[] cipherTextAsByteArray = new byte[gcmBlockCipher.GetOutputSize(dataForEncryption.Length)];
            int length = gcmBlockCipher.ProcessBytes(dataForEncryption, 0, dataForEncryption.Length, cipherTextAsByteArray, 0);
            gcmBlockCipher.DoFinal(cipherTextAsByteArray, length);
            byte[] output = null;
            //Put the pices of the message together.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(salt);
                    binaryWriter.Write(nonce);
                    binaryWriter.Write(cipherTextAsByteArray);
                }
                output = memoryStream.ToArray();
            }
            File.WriteAllBytes("encryptedFile.enc", output);
            #endregion

            #region DECRYPTION
            byte[] encryptedMessageAsByteArray = File.ReadAllBytes("encryptedFile.enc");
            
            salt = new byte[128 / 8];
            Array.Copy(encryptedMessageAsByteArray, salt, salt.Length);

            using (MemoryStream memoryStream = new MemoryStream(encryptedMessageAsByteArray))
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                salt = binaryReader.ReadBytes(salt.Length);
                nonce = binaryReader.ReadBytes(128 / 8);
                gcmBlockCipher = new GcmBlockCipher(new AesEngine());
                aeadParameters = new AeadParameters(new KeyParameter(key.GetKey()), 128, nonce, salt);
                gcmBlockCipher.Init(false, aeadParameters);

                cipherTextAsByteArray = binaryReader.ReadBytes(encryptedMessageAsByteArray.Length - salt.Length - nonce.Length);
                byte[] decryptedTextAsByteArray = new byte[gcmBlockCipher.GetOutputSize(cipherTextAsByteArray.Length)];

                try
                {
                    //Authentication check.
                    length = gcmBlockCipher.ProcessBytes(cipherTextAsByteArray, 0, cipherTextAsByteArray.Length, decryptedTextAsByteArray, 0);
                    gcmBlockCipher.DoFinal(decryptedTextAsByteArray, length);

                }
                catch (InvalidCipherTextException e)
                {
                    //Authentication failed.
                    Log.Error("Error: {0}", e.Message);
                    return null;
                }
                Log.Information("Decrypted file content and original plain text are the same: {0}", plainText.Equals(Encoding.UTF8.GetString(decryptedTextAsByteArray)));
                return Encoding.UTF8.GetString(decryptedTextAsByteArray);
                #endregion
            }

            
        }

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information(DemonstrateFileEncryption("Multiline text:\nMultiline text:\n", "ThePasswordToDecryptAndEncryptTheFile"));
        }

    }
}