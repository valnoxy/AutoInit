using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoInit
{
    internal class Logger
    {
        private static string logFile;

        public static string StartLogging()
        {
            // Initialize logging
            DateTime today = DateTime.Now;
            string currentDate = today.ToString("dd.MM.yyyy");
            string currentTime = today.ToString("HHmm");
            string currentTimeWS = today.ToString("HH:mm");
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string appver = appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision;

            string logPath = AppContext.BaseDirectory;
            string logName = $"AutoInit_{currentDate}_{currentTime}.log";
            logFile = Path.Combine(logPath, logName);
            
            using (StreamWriter streamWriter = new StreamWriter(logFile))
            {
                string introMsg =
@$"AutoInit started at {currentDate} {currentTimeWS}
Log file: {logFile}
===========================================================

App Version: {appver}
Windows Version: {Environment.OSVersion}

===========================================================";
                streamWriter.WriteLine(introMsg);
                streamWriter.Close();
            }

            return logFile;
        }
        
        public static void Log(string message)
        {
            if (string.IsNullOrEmpty(message)) { return; }

            TextWriter tW = new StreamWriter(logFile, true);
            {
                DateTime today = DateTime.UtcNow;
                string currentDate = today.ToString("dd.MM.yyyy");
                string currentTimeWS = today.ToString("HH:mm");

                string msg = $"[{currentDate} {currentTimeWS}] {message}";
                tW.WriteLine(msg);
                tW.Close();
            }
        }
    }
}
