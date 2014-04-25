using System;
using System.IO;

namespace IISRedirection
{
    public class Program
    {

        private const string xmlFormat = @"<configuration>
    <configSections>
        <section name=""configurationRedirection"" />
    </configSections>
    <configProtectedData>
        <providers>
            <add name=""IISRsaProvider"" type="""" description=""Uses RsaCryptoServiceProvider to encrypt and decrypt"" keyContainerName=""iisConfigurationKey"" cspProviderName="""" useMachineContainer=""true"" useOAEP=""false"" />
        </providers>
    </configProtectedData>
    <configurationRedirection enabled=""true"" path=""{0}"" />
</configuration>";

        private const string TargetPath = @"%windir%\system32\inetsrv\config\redirection.config";

        public static int Main(string[] args)
        {
            if (args == null || args.Length != 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Usage: iisredirection <path>");
                return 1;
            }

            var tempFile = Path.GetTempFileName();
            var path = args[0].TrimEnd(new[] { '"', ' ' });
            using (var stream = File.CreateText(tempFile))
            {
                
                stream.Write(xmlFormat, path);
            }

            File.Copy(tempFile, Environment.ExpandEnvironmentVariables(TargetPath), true);
            File.Delete(tempFile);
        
            return 0;
        }
    }
}