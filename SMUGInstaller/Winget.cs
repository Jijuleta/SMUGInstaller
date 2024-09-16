using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SMUGInstaller
{
    internal class Winget
    {
        public void InstallWinGet()
        {
            string tempPath = "C:\\Temp";

            string scriptContent = $@"
                $ProgressPreference = 'SilentlyContinue';
                New-Item -Path '{tempPath}' -ItemType Directory -Force;
                Set-Location -Path '{tempPath}';
                Invoke-WebRequest -Uri https://aka.ms/getwinget -OutFile '{Path.Combine(tempPath, "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle")}';
                Invoke-WebRequest -Uri https://aka.ms/Microsoft.VCLibs.x64.14.00.Desktop.appx -OutFile '{Path.Combine(tempPath, "Microsoft.VCLibs.x64.14.00.Desktop.appx")}';
                Invoke-WebRequest -Uri https://github.com/microsoft/microsoft-ui-xaml/releases/download/v2.8.6/Microsoft.UI.Xaml.2.8.x64.appx -OutFile '{Path.Combine(tempPath, "Microsoft.UI.Xaml.2.8.x64.appx")}';
                Add-AppxPackage '{Path.Combine(tempPath, "Microsoft.VCLibs.x64.14.00.Desktop.appx")}';
                Add-AppxPackage '{Path.Combine(tempPath, "Microsoft.UI.Xaml.2.8.x64.appx")}';
                Add-AppxPackage '{Path.Combine(tempPath, "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle")}';
                
            ";

            string output = executeScript(scriptContent);
            System.Diagnostics.Debug.WriteLine(output);

            string cleanupScript = $@"
                Start-Sleep -Seconds 3;
                Remove-Item -Path '{tempPath}' -Recurse -Force;
                ";

            string cleanOutput = executeScript(cleanupScript);
            System.Diagnostics.Debug.WriteLine(cleanOutput);
        }

        public string executeScript(string scriptContent)
        {
            var scriptArguments = $"-ExecutionPolicy Bypass -Command \"{scriptContent}\"";
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                System.Diagnostics.Debug.WriteLine("Error: " + error);
            }

            return output;
        }
    }
}
