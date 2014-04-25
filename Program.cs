using System;
using System.IO;

namespace IISRedirection
{
    public class Program
    {

        private const string xmlFormat = @"
<configuration>
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
        
        private const string TargetPath32 = @"%windir%\system32\inetsrv\config\redirection.config";
        private const string TargetPath64 = @"%windir%\sysWOW64\inetsrv\config\redirection.config";
        

        public static int Main(string[] args)
        {
            if (args == null || args.Length != 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Usage: iisredirection <path>");
                return 1;
            }

            var tempFile = Path.GetTempFileName();
            using (var stream = File.CreateText(tempFile))
            {
                stream.Write(string.Format(xmlFormat, args[0]));
            }
            File.Copy(tempFile, Environment.ExpandEnvironmentVariables(TargetPath32), true);
            File.Copy(tempFile, Environment.ExpandEnvironmentVariables(TargetPath64), true);
            File.Delete(tempFile);
            
            return 0;
        }
    }
}
