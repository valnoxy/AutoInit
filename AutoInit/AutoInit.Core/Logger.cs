using System.Reflection;

namespace AutoInit.Core
{
    public class Logger
    {
        private static string logFile;

        public static string StartLogging(string Interface, string InterfaceVersion)
        {
            // Initialize logging
            DateTime today = DateTime.Now;
            string currentDate = today.ToString("dd.MM.yyyy");
            string currentTime = today.ToString("HHmm");
            string currentTimeWS = today.ToString("HH:mm");
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string appver = appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision;

            string logPath = Path.Combine(AppContext.BaseDirectory, "logs");
            string logName = $"AutoInit_{Interface}_{currentDate}_{currentTime}.log";
            logFile = Path.Combine(logPath, logName);

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            using (StreamWriter streamWriter = new StreamWriter(logFile))
            {
                string introMsg =
@$"AutoInit started at {currentDate} {currentTimeWS}
Log file: {logFile}
===========================================================

Core Version: {appver}
AutoInit {Interface} Version: {InterfaceVersion}
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

        public static string? GetGitHash()
        {
            string gitVersion = String.Empty;
            using Stream? stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("AutoInit." + "version.txt");
            if (stream != null)
            {
                using StreamReader reader = new StreamReader(stream);
                gitVersion = reader.ReadLine();
            }

            return gitVersion;
        }
    }
}
