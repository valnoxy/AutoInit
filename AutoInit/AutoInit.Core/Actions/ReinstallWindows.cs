using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace AutoInit.Core
{
    internal class ReinstallWindows
    {
        public static int WriteIniFile()
        {
            IniFile iniFile = new IniFile("C:\\AutoDeploya.ini");
            iniFile.Write("General", "Version", "2.0");

            string winver;
            string ver = Environment.OSVersion.ToString();

            if (ver.Contains("10.0.19")) // Windows 10
                winver = "Windows 10";
            else if (ver.Contains("10.0.22")) // Windows 11
                winver = "Windows 11";
            else // System is too old or too new (newer Windows 10 build number >19042, <22000) ... Switch to Windows 10
                winver = "Windows 10";

            iniFile.Write("General", "Image", winver);
            iniFile.Write("General", "Disk", "0");

            bool IsUEFI = IsWindowsUEFI();
            if (IsUEFI)
                iniFile.Write("General", "Firmware", "EFI");
            if (!IsUEFI)
                iniFile.Write("General", "Firmware", "BIOS");
            iniFile.Write("General", "Type", "Cloud");

            return 0;
        }

        public static int ExtractScript()
        {
            if (Directory.Exists("C:\\AutoInit"))
            {
                if (System.IO.File.Exists("C:\\AutoInit\\deploya.wim"))
                    System.IO.File.Delete("C:\\AutoInit\\deploya.wim");
                if (System.IO.File.Exists("C:\\AutoInit\\boot.sdi"))
                    System.IO.File.Delete("C:\\AutoInit\\boot.sdi");
                //System.IO.File.WriteAllText("C:\\AutoInit\\add_to_bootloader.cmd", AutoInit.Scripts.add_to_bootloader);
            }
            else
            {
                Directory.CreateDirectory("C:\\AutoInit");
                //System.IO.File.WriteAllText("C:\\AutoInit\\add_to_bootloader.cmd", AutoInit.Scripts.add_to_bootloader);
            }
            return 0;
        }

        public static int DownloadFiles()
        {
            try
            {
                WebClient bl = new();
                string bootSDI = "https://dl.exploitox.de/autoinit/bootloader/boot.sdi";
                string wimfile = "https://dl.exploitox.de/autoinit/bootloader/boot.wim";
                bl.DownloadFile(bootSDI, "C:\\AutoInit\\boot.sdi");
                bl.DownloadFile(wimfile, "C:\\AutoInit\\boot.wim");
            }
            catch 
            {
                return 1;
            }
            return 0;
        }

        #region Modules
        // See https://stackoverflow.com/questions/58823493/how-do-i-detect-if-the-current-windows-is-installed-in-uefi-or-legacy-mode
        public const int ERROR_INVALID_FUNCTION = 1;
        [DllImport("kernel32.dll",
            EntryPoint = "GetFirmwareEnvironmentVariableW",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFirmwareType(string lpName, string lpGUID, IntPtr pBuffer, uint size);

        public static bool IsWindowsUEFI()
        {
            // Call the function with a dummy variable name and a dummy variable namespace (function will fail because these don't exist.)
            GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);

            if (Marshal.GetLastWin32Error() == ERROR_INVALID_FUNCTION)
            {
                // Calling the function threw an ERROR_INVALID_FUNCTION win32 error, which gets thrown if either
                // - The mainboard doesn't support UEFI and/or
                // - Windows is installed in legacy BIOS mode
                return false;
            }
            else
            {
                // If the system supports UEFI and Windows is installed in UEFI mode it doesn't throw the above error, but a more specific UEFI error
                return true;
            }
        }
        #endregion
    }
}
