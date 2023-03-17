using Microsoft.Win32;
using System;
using System.IO;

namespace AutoInit.Boot.Common
{
    internal class Registry
    {
        public static void SetReg(string keyPath, RegistryValueKind valueKind, object valueData)
        {
            RegistryKey? regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath, true);

            if (regKey != null)
            {
                try
                {
                    regKey.SetValue(Path.GetFileName(keyPath), valueData, valueKind);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    regKey.Close();
                }
            }
        }
    }
}
