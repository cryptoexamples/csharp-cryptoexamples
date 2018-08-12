using System;
using Xunit;
using com.cryptoexamples.csharp;
using System.Security.Cryptography;
using System.Text;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private String plainText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.";
        private String plainText2 = "Text that is going to be sent over an insecure channel and must be encrypted at all costs!";

        [Fact]
        public void AsymmetricStringEncryptionTest()
        {
            Console.SetOut(ExampleAsymetricStringEncryption.LOGGER);
            ExampleAsymetricStringEncryption.Main(null);

            Assert.Equal("Decrypted and original plain text are the same: True", ExampleAsymetricStringEncryption.LOGGER.ToString());
        }

        [Fact]
        public void StringEncryptionPasswordBasedTest()
        {
            Console.SetOut(ExampleStringEncryptionPasswordBased.LOGGER);
            // Basic test if encryption and decryption is working.
            ExampleStringEncryptionPasswordBased.Main(null);
            Assert.Equal("They are the same: True", ExampleStringEncryptionPasswordBased.LOGGER.ToString());
        }

        [Fact]
        public void FileEncryptionTest()
        {
            Console.SetOut(ExampleFileEncryption.LOGGER);
            ExampleFileEncryption.Main(null);

            Assert.Equal(plainText, ExampleFileEncryption.LOGGER.ToString());
        }

        [Fact]
        public void HashingTest()
        {
            Assert.Equal("113F62C2E4F3F1CB4AE0A561914C895D75D00407E6B35AB7AF0595C136AF5604", ExampleHashing.DemonstrateHashing("Text that should be authenticated by comparing the hash of it!"));
        }

        [Fact]
        public void SigningTest()
        {
            Console.SetOut(ExampleSigning.LOGGER);
            ExampleSigning.Main(null);

            Assert.Equal("The data was verified.", ExampleSigning.LOGGER.ToString());
        }

        [Fact]
        public void StringEncryptionKeyBasedTest()
        {
            Console.SetOut(ExampleStringEncryptionKeyBasedInOneMethod.LOGGER);
            // Basic test if encryption and decryption is working.
            ExampleStringEncryptionKeyBasedInOneMethod.Main(null);
            Assert.Equal("They are the same: True", ExampleStringEncryptionKeyBasedInOneMethod.LOGGER.ToString());
        }

        [Fact]
        public void KeyStorageProviderTest()
        {
            String ContainerName = "MyContainer";
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText2);
            byte[] encryptedData;
            String decryptedString;

            CspParameters cp = new CspParameters
            {
                KeyContainerName = ContainerName
            };
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            //Encrypt
            encryptedData = rsa.Encrypt(dataToEncrypt, false);

            CspParameters getParametersFromKSP = new CspParameters
            {
                KeyContainerName = ContainerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(getParametersFromKSP);

            //Decrypt
            decryptedString = Encoding.UTF8.GetString(rsa2.Decrypt(encryptedData, false));

            Assert.Equal(decryptedString, plainText2);
        }
    }
}
