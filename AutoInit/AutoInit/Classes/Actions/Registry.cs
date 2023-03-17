using Microsoft.Win32;
using System;
using System.IO;

namespace AutoInit.Classes.Actions
{
    internal class Registry
    {
        public static void SetReg(string keyPath, string keyName, RegistryValueKind valueKind, object valueData)
        {
            var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath, true);

            if (regKey != null)
            {
                try
                {
                    Logger.Log("Setting value ...");
                    regKey.SetValue(keyName, valueData, valueKind);
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
            else
                Logger.Log("Error setting the registry key: regKey is null");
        }
    }
}
