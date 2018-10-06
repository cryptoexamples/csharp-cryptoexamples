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

        public static readonly string inputFile = @"encryptedFile.enc";

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information(DemonstrateFileEncryption("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", "ThePasswordToDecryptAndEncryptTheFile"));
        }

        public static String DemonstrateFileEncryption(String plainText, String password, byte[] nonSecretPayload = null, int nonSecretPayloadLength = 0)
        {
            //----------------------------Encrypt----------------------------


            byte[] secretMessage = Encoding.UTF8.GetBytes(plainText);
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            var generator = new Pkcs5S2ParametersGenerator();

            //Use Random Salt to minimize pre-generated weak password attacks.
            var salt = new byte[SaltBitSize / 8];
            Random.NextBytes(salt);

            generator.Init(
                PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
                salt,
                Iterations);

            //Generate Key
            var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            //Create Full Non Secret Payload
            var payload = new byte[salt.Length + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

            nonSecretPayload = payload;
            //Using random nonce large enough not to repeat
            var nonce = new byte[NonceBitSize / 8];
            Random.NextBytes(nonce, 0, nonce.Length);

            var cipher = new GcmBlockCipher(new AesFastEngine());
            var parameters = new AeadParameters(new KeyParameter(key.GetKey()), MacBitSize, nonce, nonSecretPayload);
            cipher.Init(true, parameters);

            //Generate Cipher Text With Auth Tag
            var cipherText = new byte[cipher.GetOutputSize(secretMessage.Length)];
            var len = cipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0);
            cipher.DoFinal(cipherText, len);
            byte[] output = null;
            //Assemble Message
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    //Prepend Authenticated Payload
                    binaryWriter.Write(nonSecretPayload);
                    //Prepend Nonce
                    binaryWriter.Write(nonce);
                    //Write Cipher Text
                    binaryWriter.Write(cipherText);
                }
                output = combinedStream.ToArray();
            }
            File.WriteAllBytes(inputFile, output);


            //----------------------------Decrypt----------------------------


            var encryptedMessage = File.ReadAllBytes(inputFile);
            generator = new Pkcs5S2ParametersGenerator();

            //Grab Salt from Payload
            salt = new byte[SaltBitSize / 8];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length);

            generator.Init(
                PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
                salt,
                Iterations);

            //Generate Key
            key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            nonSecretPayloadLength += salt.Length;

            using (var cipherStream = new MemoryStream(encryptedMessage))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                //Grab Payload
                nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

                //Grab Nonce
                nonce = cipherReader.ReadBytes(NonceBitSize / 8);

                cipher = new GcmBlockCipher(new AesFastEngine());
                parameters = new AeadParameters(new KeyParameter(key.GetKey()), MacBitSize, nonce, nonSecretPayload);
                cipher.Init(false, parameters);

                //Decrypt Cipher Text
                cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
                byte[] decryptedTextAsByteArray = new byte[cipher.GetOutputSize(cipherText.Length)];

                try
                {
                    len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, decryptedTextAsByteArray, 0);
                    cipher.DoFinal(decryptedTextAsByteArray, len);

                }
                catch (InvalidCipherTextException e)
                {
                    //Return null if it doesn't authenticate
                    Log.Error("Error: {0}", e.Message);
                    return null;
                }
                Log.Information("Decrypted file content and original plain text are the same: {0}", plainText.Equals(Encoding.UTF8.GetString(decryptedTextAsByteArray)));
                return Encoding.UTF8.GetString(decryptedTextAsByteArray);
            }
        }
    }
}