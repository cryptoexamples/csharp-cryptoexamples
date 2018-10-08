using System;
using System.Security.Cryptography;
using System.IO;
using Serilog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;

namespace com.cryptoexamples.csharp
{
    /// <summary>
    /// Example for encryption and decryption of a file in one method.
    /// <para>- PKCS#5 Password-Based Cryptography Specification V2.0</para>
    /// <para>- Random salt generation</para>
    /// <para>- Payload for the authentification</para>
    /// <para>- AES-256 authenticated encryption using GCM</para>
    /// <para>- BASE64-encoding as representation for the byte-arrays</para>
    /// For more information about the used cryptosystem look at: <see href="https://en.wikipedia.org/wiki/Advanced_Encryption_Standard" />
    /// </summary>
    public static class ExampleFileEncryption
    {
        private static readonly SecureRandom Random = new SecureRandom();
        //Preconfigured Encryption Parameters
        public static readonly int NonceBitSize = 128;
        public static readonly int MacBitSize = 128;
        public static readonly int KeyBitSize = 256;

        //Preconfigured Password Key Derivation Parameters
        public static readonly int SaltBitSize = 128;
        public static readonly int Iterations = 10000;
        
        //The path and filename of the encrypted file.
        public static readonly string inputFile = @"encryptedFile.enc";

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information(DemonstrateFileEncryption("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", "ThePasswordToDecryptAndEncryptTheFile"));
        }

        public static string DemonstrateFileEncryption(string plainText, string password, byte[] nonSecretPayload = null, int nonSecretPayloadLength = 0)
        {
            //----------------------------Encrypt----------------------------

            //Contains the binary representation of the string that should be encrypted.
            byte[] dataForEncryption = Encoding.UTF8.GetBytes(plainText);
            //If the default value for nonSecretPayload is used, initialize it.
            nonSecretPayload = nonSecretPayload ?? new byte[] { };
            //Create PKCS#5-Parameters.
            Pkcs5S2ParametersGenerator pkcs5S2ParametersGenerator = new Pkcs5S2ParametersGenerator();

            //Initialize random salt.
            byte[] salt = new byte[SaltBitSize / 8];
            Random.NextBytes(salt);

            pkcs5S2ParametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt, Iterations);

            //Generate key.
            KeyParameter key = (KeyParameter)pkcs5S2ParametersGenerator.GenerateDerivedMacParameters(KeyBitSize);

            //Create payload.
            byte[] payload = new byte[salt.Length + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

            //Using random nonce.
            byte[] nonce = new byte[NonceBitSize / 8];
            Random.NextBytes(nonce, 0, nonce.Length);
            //Generate GCM block cipher and AEAD-Parameters.
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(new KeyParameter(key.GetKey()), MacBitSize, nonce, payload);
            gcmBlockCipher.Init(true, aeadParameters);

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
                    //Add authenticated payload (payload = nonSecretPayload + salt).
                    binaryWriter.Write(payload);
                    //Add nonce.
                    binaryWriter.Write(nonce);
                    //Add ciphertext.
                    binaryWriter.Write(cipherTextAsByteArray);
                }
                output = memoryStream.ToArray();
            }
            //Write the output to the file.
            File.WriteAllBytes(inputFile, output);


            //----------------------------Decrypt----------------------------

            //Read the encrypted file.
            byte[] encryptedMessageAsByteArray = File.ReadAllBytes(inputFile);
            //Create PKCS#5-Parameters.
            pkcs5S2ParametersGenerator = new Pkcs5S2ParametersGenerator();

            //Get salt from payload.
            salt = new byte[SaltBitSize / 8];
            Array.Copy(encryptedMessageAsByteArray, nonSecretPayloadLength, salt, 0, salt.Length);

            pkcs5S2ParametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt, Iterations);

            //Generate key.
            key = (KeyParameter)pkcs5S2ParametersGenerator.GenerateDerivedMacParameters(KeyBitSize);
            //Calculate the size of the payload length.
            nonSecretPayloadLength += salt.Length;

            using (MemoryStream memoryStream = new MemoryStream(encryptedMessageAsByteArray))
            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                //Get the payload (nonSecretPayload + salt).
                payload = binaryReader.ReadBytes(nonSecretPayloadLength);

                //Get nonce.
                nonce = binaryReader.ReadBytes(NonceBitSize / 8);
                //Generate GCM block cipher and AEAD-Parameters.
                gcmBlockCipher = new GcmBlockCipher(new AesEngine());
                aeadParameters = new AeadParameters(new KeyParameter(key.GetKey()), MacBitSize, nonce, payload);
                gcmBlockCipher.Init(false, aeadParameters);

                //Decrypt ciphertext.
                cipherTextAsByteArray = binaryReader.ReadBytes(encryptedMessageAsByteArray.Length - nonSecretPayloadLength - nonce.Length);
                byte[] decryptedTextAsByteArray = new byte[gcmBlockCipher.GetOutputSize(cipherTextAsByteArray.Length)];

                try
                {
                    //Authentication check.
                    length = gcmBlockCipher.ProcessBytes(cipherTextAsByteArray, 0, cipherTextAsByteArray.Length, decryptedTextAsByteArray, 0);
                    gcmBlockCipher.DoFinal(decryptedTextAsByteArray, length);

                }
                catch (InvalidCipherTextException e)
                {
                    //Return null if it doesn't authenticate.
                    Log.Error("Error: {0}", e.Message);
                    return null;
                }
                Log.Information("Decrypted file content and original plain text are the same: {0}", plainText.Equals(Encoding.UTF8.GetString(decryptedTextAsByteArray)));
                return Encoding.UTF8.GetString(decryptedTextAsByteArray);
            }
        }
    }
}