using System;
using System.Security.Cryptography;

namespace csharp_cryptoexamples.src
{
    public class ExmapleKeyStorageProvider
    {
        public static void Main()
        {
            DemonstrateKeyStorage("MyKeyContainerName");
        }

        public static RSACryptoServiceProvider DemonstrateKeyStorage(String ContainerName)
        {
            //---- Fill Parameter in KSP ---------

            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters
            {
                KeyContainerName = ContainerName
            };

            //---- Get Parameter from KSP -------------------------

            // If the key is allways in use, the CspParamters will set to the safed one.
            CspParameters getParametersFromKSP = new CspParameters
            {
                KeyContainerName = ContainerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(getParametersFromKSP);

            return rsa2;
        }
    }
}
