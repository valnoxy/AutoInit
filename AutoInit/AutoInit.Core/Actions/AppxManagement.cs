using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace AutoInit.Core.Actions
{
    public class AppxManagement
    {
        // public list of apps to remove
        public class App
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
        public static List<App> apps;

        /// <summary>
        /// Removes the App from the system.
        /// </summary>
        /// <remarks>
        /// Return code: 0 = success, 1 = failure
        /// </remarks>
        /// <param name="appID">Application ID</param>
        /// <returns>Status code</returns>
        public static int RemoveAppx(string appID)
        {
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.CreateNoWindow = false;
            psi.Arguments = $"Get-AppxPackage '{appID}' | Remove-AppxPackage";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = "powershell.exe";
            var proc = Process.Start(psi);

            proc.WaitForExit();
            return proc.ExitCode;
        }

        /// <summary>
        /// Checks if WinGet is installed on the system.
        /// </summary>
        /// <returns>Summary as bool</returns>
        public static bool IsWingetInstalled()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c winget";
            p.Start();
            p.WaitForExit();

            return p.ExitCode == 0;
        }

        /// <summary>
        /// Installs the App to the system via WinGet.
        /// </summary>
        /// <param name="PackageName"></param>
        /// <returns>Status code</returns>
        public static int InstallApp(string PackageName)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c winget install --id {PackageName} --accept-source-agreements --accept-package-agreements";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            return p.ExitCode;
        }

        /// <summary>
        /// Download the desired file from the internet.
        /// </summary>
        /// <param name="DownloadURL"></param>
        /// <param name="SaveTo"></param>
        /// <returns>Status code</returns>
        public static int InstallRM(string DownloadURL, string SaveTo)
        {
            try
            {
                WebClient rms = new();
                rms.DownloadFile(DownloadURL, SaveTo);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Installs the desired Feature on the system.
        /// </summary>
        /// <param name="FeatureName"></param>
        /// <returns></returns>
        public static int InstallFeature(string FeatureName)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c \"dism /Online /Enable-Feature /All /FeatureName:{FeatureName} /NoRestart\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            return p.ExitCode;
        }
    }
}
