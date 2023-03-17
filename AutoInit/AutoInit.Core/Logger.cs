using System.Reflection;

namespace AutoInit.Core
{
    public class Logger
    {
        private static string logFile;

        public static string StartLogging(string @interface, string interfaceVersion)
        {
            // Initialize logging
            var today = DateTime.Now;
            var currentDate = today.ToString("dd.MM.yyyy");
            var currentTime = today.ToString("HHmm");
            var currentTimeWs = today.ToString("HH:mm");
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var appver = appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision;

            var logPath = Path.Combine(AppContext.BaseDirectory, "logs");
            var logName = $"AutoInit_{@interface}_{currentDate}_{currentTime}.log";
            logFile = Path.Combine(logPath, logName);

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            using (var streamWriter = new StreamWriter(logFile))
            {
                var introMsg =
@$"AutoInit started at {currentDate} {currentTimeWs}
Log file: {logFile}
===========================================================

Core Version: {appver}
AutoInit {@interface} Version: {interfaceVersion}
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
                var today = DateTime.UtcNow;
                var currentDate = today.ToString("dd.MM.yyyy");
                var currentTimeWs = today.ToString("HH:mm");

                var msg = $"[{currentDate} {currentTimeWs}] {message}";
                tW.WriteLine(msg);
                tW.Close();
            }
        }
    }
}
