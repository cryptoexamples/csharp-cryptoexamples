using System;
using Xunit;
using com.cryptoexamples.csharp;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
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

            Assert.Equal("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.", ExampleFileEncryption.LOGGER.ToString());
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
    }
}
