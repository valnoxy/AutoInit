using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInit.Core
{
    public class Configuration
    {
        public static string ConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        // AutoInit
        public static string AdminPassword { get; set; }
        public static string DefaultUsername { get; set; }
        public static bool RemoveDefaultUser { get; set; }
        public static bool BackgroundMusic { get; set; }

        // Applications
        public static string PackageID_Firefox { get; set; }
        public static string PackageID_AcrobatReader { get; set; }
        public static bool InstallFirefox { get; set; }
        public static bool InstallAcrobatReader { get; set; }
        public static bool EnableSMB1 { get; set; }
        public static bool EnableNET35 { get; set; }
        public static bool RemoteMaintenance { get; set; }
        public static string RemoteMaintenanceURL { get; set; }
        public static string RemoteMaintenanceFileName { get; set; }
        public static string OtherApps { get; set; }

        // Settings
        public static bool DisableTelemetry { get; set; }
        public static bool CheckActivation { get; set; }
        public static string SystemProtection { get; set; }
        public static bool DisableAutoRebootAfterBSOD { get; set; }
        public static string SPMaxMemoryDump { get; set; }
        public static bool SetMaxPerformance { get; set; }
        public static bool DisableFastBoot { get; set; }
    }
}
