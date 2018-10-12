using System;
using Xunit;
using com.cryptoexamples.csharp;
using System.IO;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly string plainText = "Multiline text:\nMultiline text:\n";
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
            Assert.Contains("Decrypted and original plain text are the same: True", outMessage.ToString());
            //Catch coverage
            Refresh();
            ExampleStringEncryptionPasswordBased.DemonstrateStringEncryptionPasswordBased(null, null);
            Assert.Contains("Error", outMessage.ToString());
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
            Refresh();
            Assert.Equal("VGV4dCB0aGF0IHNob3VsZCBiZSBhdXRoZW50aWNhdGVkIGJ5IGNvbXBhcmluZyB0aGUgaGFzaCBvZiBpdCE=", ExampleHashing.DemonstrateHashing("Text that should be authenticated by comparing the hash of it!"));
        }

        [Fact]
        public void SigningTest()
        {
            Refresh();
            ExampleSignature.Main();
            Assert.Contains("Signature is correct: True", outMessage.ToString());
        }

        [Fact]
        public void StringEncryptionKeyBasedTest()
        {
            Refresh();
            // Basic test if encryption and decryption is working.
            ExampleStringEncryptionKeyBased.Main();
            Assert.Contains("Decrypted and original plain text are the same: True", outMessage.ToString());
        }

        /*[Fact]
        public void KeyStorageProviderTest()
        {
            string ContainerName = "MyContainer";
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText2);
            byte[] encryptedData;
            string decryptedString;

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
