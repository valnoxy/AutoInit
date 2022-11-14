using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using static AutoInit.Program.AutoInit;

namespace AutoInit
{
    internal class ConfigureWindows
    {
        #region Telemetry
        public static bool DisableCEI()
        {
            // CEIP/SQM
            bool CEIP = SetRegkey("Software\\Policies\\Microsoft\\SQMClient\\Windows", "CEIPEnable", "0");
            return CEIP;
        }

        public static bool DisableAIT()
        {
            // Disable AIT
            bool CEIP = SetRegkey("Software\\Policies\\Microsoft\\Windows\\AppCompat", "AITEnable", "0");
            return CEIP;
        }

        public static bool DisableCEIP()
        {
            // Disable CEIP
            bool CEIP = true;
            int a = RunProcess("schtasks.exe", "/change /TN \"\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /DISABLE");

            int b = RunProcess("schtasks.exe", "/change /TN \"\\Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask\" /DISABLE");
            
            int c = RunProcess("schtasks.exe", "/change /TN \"\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip\" /DISABLE");

            return CEIP;
        }

        public static bool DisableDCP()
        {
            // Disable DCP
            bool DCP = true;
            bool a = SetRegkey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", "0");
            if (a == false) DCP = false;

            bool b = SetRegkey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", "0");
            if (b == false) DCP = false;

            bool c = SetRegkey("SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", "AllowTelemetry", "0");
            if (c == false) DCP = false;

            bool d = SetRegkey("SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", "LimitEnhancedDiagnosticDataWindowsAnalytics", "0");
            if (d == false) DCP = false;

            bool e = SetRegkey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", "0");
            if (e == false) DCP = false;

            return DCP;
        }

        public static bool DisableLicenseTel()
        {
            // Disable LT
            bool DCP = true;
            bool a = SetRegkey("Software\\Policies\\Microsoft\\Windows NT\\CurrentVersion\\Software Protection Platform", "NoGenTicket", "1");
            if (a == false) DCP = false;

            return DCP;
        }

        public static bool DisableDeviceSensus()
        {
            // Disable LT
            bool status = true;
            int a = RunProcess("schtasks.exe", "/change /TN \"Microsoft\\Windows\\Device Information\\Device\" /disable");
            if (a != 0) status = false;

            return status;
        }
        #endregion

        #region System check
        public static bool IsWindowsActivated()
        {
            ManagementScope scope = new ManagementScope(@"\\" + System.Environment.MachineName + @"\root\cimv2");
            scope.Connect();

            SelectQuery searchQuery = new SelectQuery("SELECT * FROM SoftwareLicensingProduct WHERE ApplicationID = '55c92734-d682-4d71-983e-d6ec3f16059f' and LicenseStatus = 1");
            ManagementObjectSearcher searcherObj = new ManagementObjectSearcher(scope, searchQuery);

            using (ManagementObjectCollection obj = searcherObj.Get())
            {
                return obj.Count > 0;
            }
        }
        #endregion

        #region Tweaks
        public static bool DisableAutoRebootOnBSOD()
        {
            bool status = true;
            bool a = SetRegkey("SYSTEM\\CurrentControlSet\\Control\\CrashControl", "AutoReboot","0");
            if (a == false) status = false;
            return status;
        }

        public static bool SetMaxMemDump()
        {
            bool status = true;
            string DebugInfoType = "";

            if (Core.Configuration.SPMaxMemoryDump == "None")
                DebugInfoType = "0";
            else if (Core.Configuration.SPMaxMemoryDump == "Complete")
                DebugInfoType = "1";
            else if (Core.Configuration.SPMaxMemoryDump == "Kernel")
                DebugInfoType = "2";
            else if (Core.Configuration.SPMaxMemoryDump == "Small")
                DebugInfoType = "3";
            else if (Core.Configuration.SPMaxMemoryDump == "Auto")
                DebugInfoType = "7";

            int a = RunProcess("wmic.exe", $"recoveros set DebugInfoType = {DebugInfoType}"); 
            if (a != 0) status = false;
            return status;
        }

        public static bool ShadowStorage()
        {
            bool status = true;
            int a = RunProcess("vssadmin.exe", "resize shadowstorage /for=C: /on=C: /maxsize=20%"); 
            if (a != 0) status = false;
            return status;
        }

        public static bool SetMaxPerformance()
        {
            bool status = true;
            int a = RunProcess("powercfg.exe", "/setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
            if (a != 0) status = false;
            return status;
        }
        
        public static bool DisableFastBoot()
        {
            bool status = true;
            int a = RunProcess("powercfg.exe", "/hibernate off");
            if (a != 0) status = false;
            return status;
        }
        #endregion

        #region Modules
        private static bool SetRegkey(string path, string name, string value)
        {
            try
            {
                RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                var reg = localMachine.OpenSubKey(path, true) ?? localMachine.CreateSubKey(path);
                reg.SetValue(name, value);
            }
            catch { return false; }
            return true;
        }
        
        private static int RunProcess(string program, string argument)
        {
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.CreateNoWindow = false;
            psi.Arguments = argument;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = program;
            var proc = Process.Start(psi);

            proc.WaitForExit();
            return proc.ExitCode;
        }
        #endregion
    }
}
