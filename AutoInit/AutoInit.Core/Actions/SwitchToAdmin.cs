using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInit.Core.Actions
{
    public class SwitchToAdmin
    {
        /// <summary>
        /// Enables the Administrator account.
        /// </summary>
        /// <remarks>
        /// Return code: 0 = success, 1 = failure
        /// </remarks>
        /// <returns>Exitcode</returns>
        public static int EnableAdmin()
        {
            var p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c \"net user Administrator /active:yes\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }

        /// <summary>
        /// Changes the Password of the Administrator.
        /// </summary>
        /// <remarks>
        /// Return code: 0 = success, 1 = failure
        /// </remarks>
        /// <returns>Exitcode</returns>
        public static int UpdateAdminPassword(string password)
        {
            var p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c \"net user Administrator {password}\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }

        /// <summary>
        /// Removes the User from the system.
        /// </summary>
        /// <remarks>
        /// Return code: 0 = success, 1 = failure
        /// </remarks>
        /// <returns>Exitcode</returns>
        public static int RemoveUser(string username)
        {
            var p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c \"net user {username} /delete\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }
    }
}
