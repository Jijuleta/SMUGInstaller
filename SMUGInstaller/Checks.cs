using System.Diagnostics;
using System.Security.Principal;

namespace SMUGInstaller
{
    internal class Checks
    {
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool IsWinGet()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c winget";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            System.Diagnostics.Debug.WriteLine(output);
            System.Diagnostics.Debug.WriteLine(error);

            if (!string.IsNullOrEmpty(error))
            {
                return false;
            }

            return true;
        }

        public static bool IsRightWinGet()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c winget -v";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            System.Diagnostics.Debug.WriteLine(output);
            System.Diagnostics.Debug.WriteLine(error);

            string versionString = output.Trim();

            if (versionString.StartsWith("v"))
            {
                versionString = versionString.Substring(1);
            }

            string[] versionParts = versionString.Split('.');

            int majorVersion;
            int minorVersion;
            if (!int.TryParse(versionParts[0], out majorVersion) || (!int.TryParse(versionParts[1], out minorVersion)))
            {
                return false;
            }

            if (majorVersion < 1 || (majorVersion == 1 && minorVersion < 8))
            {
                return false;
            }

            return true;
        }

    }
}
