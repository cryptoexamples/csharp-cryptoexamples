using System;
using Xunit;
using com.cryptoexamples.csharp;
using System.IO;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly String plainText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.";
        private StringWriter outMessage;
        private StringWriter errMessage;
        
        private void Refresh()
        {
            outMessage = new StringWriter();
            errMessage = new StringWriter();
            Console.SetOut(outMessage);
            Console.SetError(errMessage);
        }

        [Fact]
        public void AsymmetricStringEncryptionTest()
        {
            Refresh();
            ExampleAsymetricStringEncryption.Main();
            Assert.Contains("Decrypted and original plain text are the same: True", outMessage.ToString());
        }

        [Fact]
        public void StringEncryptionPasswordBasedTest()
        {
            Refresh();
            // Basic test if encryption and decryption is working.
            ExampleStringEncryptionPasswordBased.Main();
            Assert.Contains("They are the same: True", outMessage.ToString());
        }

        [Fact]
        public void FileEncryptionTest()
        {
            Refresh();
            ExampleFileEncryption.Main();
            Assert.Contains(plainText, outMessage.ToString());
        }

        [Fact]
        public void HashingTest()
        {
            Assert.Equal("113F62C2E4F3F1CB4AE0A561914C895D75D00407E6B35AB7AF0595C136AF5604", ExampleHashing.DemonstrateHashing("Text that should be authenticated by comparing the hash of it!"));
        }

        [Fact]
        public void SigningTest()
        {
            Refresh();
            ExampleSigning.Main();
            Assert.Contains("The data was verified.", outMessage.ToString());
        }

        [Fact]
        public void StringEncryptionKeyBasedTest()
        {
            Refresh();
            // Basic test if encryption and decryption is working.
            ExampleStringEncryptionKeyBasedInOneMethod.Main();
            Assert.Contains("They are the same: True", outMessage.ToString());
        }

        /*[Fact]
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
        }*/
    }
}
