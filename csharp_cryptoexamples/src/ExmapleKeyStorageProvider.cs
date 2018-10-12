using System;
using System.Security.Cryptography;
using Serilog;

namespace csharp_cryptoexamples.src
{
    public static class ExmapleKeyStorageProvider
    {
        public static RSACryptoServiceProvider DemonstrateKeyStorage(string ContainerName)
        {
            //---- Fill Parameter in KSP ---------

            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            CspParameters cspParameters = new CspParameters
            {
                KeyContainerName = ContainerName
            };

            //---- Get Parameter from KSP -------------------------

            // If the key is allways in use, the CspParamters will set to the safed one.
            CspParameters cspParametersFromKSP = new CspParameters
            {
                KeyContainerName = ContainerName
            };

            RSACryptoServiceProvider rSACryptoServiceProvider = null;

            try
            {
                // Create a new instance of RSACryptoServiceProvider that accesses  
                // the key container MyKeyContainerName.  
                rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParametersFromKSP);
            }
            catch (CryptographicException e)
            {
                Log.Error("Error: {0}", e.Message);
                return null;
            }
            Log.Information("The password before and after keystorage are the same: {0}", cspParameters.KeyPassword.Equals(cspParametersFromKSP.KeyPassword));
            return rSACryptoServiceProvider;
        }

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DemonstrateKeyStorage("MyKeyContainerName");
        }

    }
}
