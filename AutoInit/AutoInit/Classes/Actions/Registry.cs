using Microsoft.Win32;
using System;
using System.IO;

namespace AutoInit.Classes.Actions
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
                    Logger.Log("Error setting the registry key: " + ex.Message);
                }
                finally
                {
                    regKey.Close();
                }
            }
        }
    }
}
