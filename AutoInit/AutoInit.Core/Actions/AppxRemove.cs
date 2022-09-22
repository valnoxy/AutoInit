using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;


namespace AutoInit.Core
{
    public class AppxRemove
    {
        // public list of apps to remove
        public class App
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
        public static List<App> apps;

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
    }
}
