using System;
using System.Security.Cryptography;
using Serilog;

namespace csharp_cryptoexamples.src
{
    public static class ExmapleKeyStorageProvider
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
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

            RSACryptoServiceProvider rsa2 = null;

            try
            {
                // Create a new instance of RSACryptoServiceProvider that accesses  
                // the key container MyKeyContainerName.  
                rsa2 = new RSACryptoServiceProvider(getParametersFromKSP);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return rsa2;
        }
    }
}
