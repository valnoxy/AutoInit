using Konsole;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoInit.Core;

namespace AutoInit
{
    public class Program
    {
        public static class AutoInit
        {
            // Main Menu switches
            static bool finished = false;
            static bool switchToAdmin = false;
            static bool removeBloadware = false;
            static bool installApplications = false;
            static bool configureWindows = false;
            static bool reinstallWindows = false;

            // Music directory
            static readonly string MusicDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music");

            // Git Version
            public static string gitVersion = String.Empty;

            // Args switches
            static bool noIntro = false;

            public static void Main(string[] args)
            {
                // Check args
                if (args != null || args.Length != 0)
                {
                    if (args.Length == 1 || args.Length == 2)
                    {
                        string arg = args[0];
                        if (args[0] == "--no-intro")
                        {
                            noIntro = true;
                        }

                        if (args[0] == "--new-config")
                        {
                            CreateNewConfig();
                            Environment.Exit(0);
                        }

                        if (args[0] == "--deployment")
                        {
                            Core.Configuration.RemoveDefaultUser = false;
                            Core.Configuration.BackgroundMusic = false;
                            Core.Configuration.EnableNET35 = true;
                            Core.Configuration.EnableSMB1 = false;
                            Core.Configuration.RemoteMaintenance = true;
                            Core.Configuration.RemoteMaintenanceURL = "https://wolkenhof.com/download/Fernwartung_Wolkenhof.exe";
                            Core.Configuration.RemoteMaintenanceFileName = "Fernwartung Wolkenhof.exe";

                            AppxRemove.RemoveAppx("Microsoft.Appconnector");
                            AppxRemove.RemoveAppx("Microsoft.549981C3F5F10");
                            AppxRemove.RemoveAppx("Microsoft.GetHelp");
                            AppxRemove.RemoveAppx("Microsoft.Getstarted");
                            AppxRemove.RemoveAppx("Microsoft.Messaging");
                            AppxRemove.RemoveAppx("Microsoft.MixedReality.Portal");
                            AppxRemove.RemoveAppx("Microsoft.WindowsFeedbackHub");
                            AppxRemove.RemoveAppx("Microsoft.WindowsAlarms");
                            AppxRemove.RemoveAppx("Microsoft.WindowsCamera");
                            AppxRemove.RemoveAppx("Microsoft.WindowsMaps");
                            AppxRemove.RemoveAppx("Microsoft.MinecraftUWP");
                            AppxRemove.RemoveAppx("Microsoft.People");
                            AppxRemove.RemoveAppx("Microsoft.Print3D");
                            AppxRemove.RemoveAppx("Microsoft.OneConnect");
                            AppxRemove.RemoveAppx("Microsoft.MicrosoftSolitaireCollection");
                            AppxRemove.RemoveAppx("Microsoft.MicrosoftStickyNotes");
                            AppxRemove.RemoveAppx("Microsoft.GroupMe10");
                            AppxRemove.RemoveAppx("Microsoft.WindowsSoundRecorder");
                            AppxRemove.RemoveAppx("Microsoft.3DBuilder");
                            AppxRemove.RemoveAppx("Microsoft.Microsoft3DViewer");
                            AppxRemove.RemoveAppx("Microsoft.BingWeather");
                            AppxRemove.RemoveAppx("Microsoft.BingSports");
                            AppxRemove.RemoveAppx("Microsoft.BingNews");
                            AppxRemove.RemoveAppx("Microsoft.BingFinance");
                            AppxRemove.RemoveAppx("Microsoft.MicrosoftOfficeHub");
                            AppxRemove.RemoveAppx("Microsoft.Office.OneNote");
                            AppxRemove.RemoveAppx("Microsoft.Office.Sway");
                            AppxRemove.RemoveAppx("Microsoft.XboxApp");
                            AppxRemove.RemoveAppx("Microsoft.Xbox.TCUI");
                            AppxRemove.RemoveAppx("Microsoft.XboxGamingOverlay");
                            AppxRemove.RemoveAppx("Microsoft.XboxGameOverlay");
                            AppxRemove.RemoveAppx("Microsoft.XboxIdentityProvider");
                            AppxRemove.RemoveAppx("Microsoft.XboxSpeechToTextOverlay");
                            AppxRemove.RemoveAppx("Microsoft.NetworkSpeedTest");
                            AppxRemove.RemoveAppx("Microsoft.Todos");
                            AppxRemove.RemoveAppx("ShazamEntertainmentLtd.Shazam");
                            AppxRemove.RemoveAppx("king.com.CandyCrushSaga");
                            AppxRemove.RemoveAppx("king.com.CandyCrushSodaSaga");
                            AppxRemove.RemoveAppx("Flipboard.Flipboard");
                            AppxRemove.RemoveAppx("9E2F88E3.Twitter");
                            AppxRemove.RemoveAppx("ClearChannelRadioDigital.iHeartRadio");
                            AppxRemove.RemoveAppx("D5EA27B7.Duolingo-LearnLanguagesforFree");
                            AppxRemove.RemoveAppx("AdobeSystemIncorporated.AdobePhotoshop");
                            AppxRemove.RemoveAppx("PandoraMediaInc.29680B314EFC2");
                            AppxRemove.RemoveAppx("46928bounde.EclipseManager");
                            AppxRemove.RemoveAppx("ActiproSoftwareLLC.562882FEEB491");
                            AppxRemove.RemoveAppx("SpotifyAB.SpotifyMusic");
                            AppxRemove.RemoveAppx("Microsoft.WindowsPhone");
                            AppxRemove.RemoveAppx("Microsoft.Windows.Phone");
                            AppxRemove.RemoveAppx("Microsoft.CommsPhone");
                            AppxRemove.RemoveAppx("Microsoft.YourPhone");
                            AppxRemove.RemoveAppx("Microsoft.RemoteDesktop");

                            try
                            {
                                WebClient rms = new();
                                string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                                string rmsFN = Path.Combine(publicDesktop, Core.Configuration.RemoteMaintenanceFileName);
                                rms.DownloadFile(Core.Configuration.RemoteMaintenanceURL, rmsFN);
                            }
                            catch {;}

                            Environment.Exit(0);
                        }

                        if (args[0] == "--update")
                        {
                            if (args.Length == 2)
                            {
                                string updatePath = args[1];
                                Console.WriteLine("[i] Updating AutoInit...");
                                Update(updatePath);
                                Console.WriteLine("[i] AutoInit updated.");
                                Environment.Exit(0);
                            }
                            Environment.Exit(1);
                        }
                    }
                }
                
                // Inititalize log file
                Logger.StartLogging();

                // Set Git version
                using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("AutoInit." + "version.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    gitVersion = reader.ReadLine();
                }
                Console.Title = "AutoInit [Build: " + gitVersion + "]";

                // Check for updates
                CheckForUpdates();

                // Verify the existance of the config file
                if (!File.Exists(Core.Configuration.ConfigurationFile))
                {
                    Logger.Log("ERROR: No configuration file found. (File missing: config.ini)");
                    Console.WriteLine("ERROR: No configuration file found. (File missing: config.ini)");
                    Console.Write("       Do you want to create a new one? (Y/N): ");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "y")
                    {
                        Console.WriteLine("[i] Creating new configuration file...");
                        CreateNewConfig();
                        Console.WriteLine("[i] New configuration file created.");
                    }
                    else
                    {
                        Console.WriteLine("Exiting...");
                        return;
                    }

                    
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }

                // Check and load the Configuration file
                InitConfiguration();

                // Initialize background music
                if (Core.Configuration.BackgroundMusic)
                {
                    var music = Task.Run(() =>
                    {
                        if (Directory.Exists(MusicDir))
                        {
                            var rand = new Random();
                            var files = Directory.GetFiles(MusicDir, "*.mp3");
                            try
                            {
                                playSound(files[rand.Next(files.Length)]);
                                Logger.Log("Playing some music.");
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[!] Cannot play audio... skipping...");
                                Logger.Log("Cannot play audio!");
                                Thread.Sleep(2000);
                                Console.ResetColor();
                            }
                        }
                    });
                }

                if (!noIntro)
                    Intro.StartIntro();
                MainMenu();
            }

            private static void Update(string updatePath)
            {
                // Copy the current AutoInit.exe to the update folder
                // Get current AutoInit.exe path
                string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                Console.WriteLine("[i] Updating now ...");
                try
                {
                    Copy(currentPath, updatePath);
                    Console.WriteLine("[i] Updating completed. Starting ...");
                    Process.Start(Path.Combine(updatePath, "AutoInit.exe"));
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Update failed! " + ex.Message);
                    Thread.Sleep(3000);
                    Console.ResetColor();
                    Environment.Exit(1);
                }
            }

            private static void CheckForUpdates()
            {
                string commits = String.Empty;

                try
                {
                    WebClient wc = new();
                    wc.Headers.Add("user-agent", "AutoInit/1.0 valnoxy.dev");
                    commits = wc.DownloadString("https://api.github.com/repos/valnoxy/AutoInit/releases");
                }
                catch
                {
                    Console.WriteLine("[i] Cannot check for updates... skipping...");
                    Logger.Log("Cannot check for updates! Failed to connect to GitHub API (commits).");
                }

                try
                {
                    // New latest version data
                    dynamic JsonCommitData = JsonConvert.DeserializeObject(commits);
                    string latestVersion = JsonCommitData[0].tag_name;
                    
                    // This version data
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                    string thisVersion = fvi.FileVersion;

                    var oldv = new Version(thisVersion);    
                    var newv = new Version(latestVersion.Substring(1, latestVersion.Length-1));    
                    
                    if (oldv < newv)
                    {
                        // Update available
                        Logger.Log($"New version available! (Current: {thisVersion} | Latest: {latestVersion})");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[i] Update available! (Current: {thisVersion} | Latest: {latestVersion})");
                        Console.Write("[i] Do you want to update? (Y/N): ");
                        string answer = Console.ReadLine();
                        if (answer.ToLower() == "y")
                        {
                            Console.WriteLine("[i] Downloading new version ...");
                            Logger.Log("Downloading new version...");

                            // Download new version (thanks to nightly.link)
                            WebClient wc = new();
                            wc.Headers.Add("user-agent", "AutoInit/1.0 valnoxy.dev");
                            string downloadUrl = $"https://github.com/valnoxy/AutoInit/releases/download/{latestVersion}/AutoInit_{latestVersion}.zip";
                            string updatePath = Path.Combine(Path.GetTempPath(), "AutoInit");
                            if (!Directory.Exists(updatePath))
                            {
                                Directory.CreateDirectory(updatePath);
                            }
                            string updateFile = Path.Combine(updatePath, "AutoInit.zip");
                            wc.DownloadFile(downloadUrl, updateFile);

                            // Extract Zip file
                            ZipFile.ExtractToDirectory(updateFile, updatePath);

                            // Delete Zip file
                            File.Delete(updateFile);

                            // Start update process
                            string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.WorkingDirectory = updatePath;
                            startInfo.FileName = "AutoInit.exe";
                            startInfo.Arguments = "--update " + appDir;
                            Process.Start(startInfo);

                            // End Process (this)
                            Environment.Exit(0);
                        }
                        Console.ResetColor();
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[i] Cannot check for updates... skipping...\n" + ex.Message);
                    Logger.Log("Failed to deserialize commits data.");
                }
            }

            private static void CreateNewConfig()
            {
                string config = @"
[AutoInit]
AdminPassword = 

; Note that this user will be deleted (if not set otherwise) after the Administration switching phase.
DefaultUsername = User
RemoveDefaultUser = true

BackgroundMusic = true

[Application]
PackageID_Firefox = Mozilla.Firefox
PackageID_AcrobatReader = Adobe.Acrobat.Reader.64-bit
InstallFirefox = true
InstallAcrobatReader = true

EnableSMB1 = true
EnableNET35 = true

RemoteMaintenance = true
RemoteMaintenanceURL = https://wolkenhof.com/download/Fernwartung_Wolkenhof.exe
RemoteMaintenanceFileName = Fernwartung Wolkenhof.exe

; Install other apps by inserting the Package ID of the application.
; You can find the Package ID by typing 'winget search <Name>' or by searching on https://winget.run/
; Use ',' for more than one application
OtherApps =  

[Settings]
DisableTelemetry = true
CheckActivation = true

; System Protection
; Set to 0% to disable system protection
SystemProtection = 20%
DisableAutoRebootAfterBSOD = true

; Options:
;   None = None
;   Complete = Complete memory dump
;   Kernel = Kernel memory dump
;   Small = Small memory dump (64 KB)
;   Auto = Automatic memory dump
SPMaxMemoryDump = Small

; Power Settings
SetMaxPerformance = true
DisableFastBoot = true";

                try
                {
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"), config);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[!] Failed to create new config: {e.Message}");
                    Environment.Exit(1);
                }
            }

            private static void InitConfiguration()
            {
                //
                // Reading Configuration
                //
                var ConfigIni = new IniFile(Core.Configuration.ConfigurationFile);
                
                Core.Configuration.AdminPassword = ConfigIni.Read("AdminPassword", "AutoInit");
                Core.Configuration.DefaultUsername = ConfigIni.Read("DefaultUsername", "AutoInit");

                if (ConfigIni.Read("RemoveDefaultUser", "AutoInit") == "true")
                    Core.Configuration.RemoveDefaultUser = true;
                else
                    Core.Configuration.RemoveDefaultUser = false;

                if (ConfigIni.Read("BackgroundMusic", "AutoInit") == "true")
                    Core.Configuration.BackgroundMusic = true;
                else
                    Core.Configuration.BackgroundMusic = false;

                Core.Configuration.PackageID_AcrobatReader = ConfigIni.Read("PackageID_AcrobatReader", "Application");
                Core.Configuration.PackageID_Firefox = ConfigIni.Read("PackageID_Firefox", "Application");

                if (ConfigIni.Read("InstallFirefox", "Application") == "true")
                    Core.Configuration.InstallFirefox = true;
                else
                    Core.Configuration.InstallFirefox = false;

                if (ConfigIni.Read("InstallAcrobatReader", "Application") == "true")
                    Core.Configuration.InstallAcrobatReader = true;
                else
                    Core.Configuration.InstallAcrobatReader = false;

                if (ConfigIni.Read("EnableSMB1", "Application") == "true")
                    Core.Configuration.EnableSMB1 = true;
                else
                    Core.Configuration.EnableSMB1 = false;

                if (ConfigIni.Read("EnableNET35", "Application") == "true")
                    Core.Configuration.EnableNET35 = true;
                else
                    Core.Configuration.EnableNET35 = false;

                if (ConfigIni.Read("RemoteMaintenance", "Application") == "true")
                    Core.Configuration.RemoteMaintenance = true;
                else
                    Core.Configuration.RemoteMaintenance = false;

                Core.Configuration.RemoteMaintenanceURL = ConfigIni.Read("RemoteMaintenanceURL", "Application");
                Core.Configuration.RemoteMaintenanceFileName = ConfigIni.Read("RemoteMaintenanceFileName", "Application");
                Core.Configuration.OtherApps = ConfigIni.Read("OtherApps", "Application");

                if (ConfigIni.Read("DisableTelemetry", "Settings") == "true")
                    Core.Configuration.DisableTelemetry = true;
                else
                    Core.Configuration.DisableTelemetry = false;

                if (ConfigIni.Read("CheckActivation", "Settings") == "true")
                    Core.Configuration.CheckActivation = true;
                else
                    Core.Configuration.CheckActivation = false;

                Core.Configuration.SystemProtection = ConfigIni.Read("SystemProtection", "Settings");

                if (ConfigIni.Read("DisableAutoRebootAfterBSOD", "Settings") == "true")
                    Core.Configuration.DisableAutoRebootAfterBSOD = true;
                else
                    Core.Configuration.DisableAutoRebootAfterBSOD = false;

                Core.Configuration.SPMaxMemoryDump = ConfigIni.Read("SPMaxMemoryDump", "Settings");

                if (ConfigIni.Read("SetMaxPerformance", "Settings") == "true")
                    Core.Configuration.SetMaxPerformance = true;
                else
                    Core.Configuration.SetMaxPerformance = false;

                if (ConfigIni.Read("DisableFastBoot", "Settings") == "true")
                    Core.Configuration.DisableFastBoot = true;
                else
                    Core.Configuration.DisableFastBoot = false;

                Logger.Log("Config loaded.");                
                
                //
                // Validating Configuration
                //
                if (Core.Configuration.AdminPassword == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No AdminPassword set)");
                    Logger.Log("ERROR: Configuation file invalid. (No AdminPassword set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.DefaultUsername == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No DefaultUsername set)");
                    Logger.Log("ERROR: Configuation file invalid. (No DefaultUsername set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.PackageID_Firefox == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No PackageID_Firefox set)");
                    Logger.Log("ERROR: Configuation file invalid. (No PackageID_Firefox set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.PackageID_AcrobatReader == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No PackageID_AcrobatReader set)");
                    Logger.Log("ERROR: Configuation file invalid. (No PackageID_AcrobatReader set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.RemoteMaintenanceURL == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No RemoteMaintenanceURL set)");
                    Logger.Log("ERROR: Configuation file invalid. (No RemoteMaintenanceURL set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.RemoteMaintenanceFileName == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No RemoteMaintenanceFileName set)");
                    Logger.Log("ERROR: Configuation file invalid. (No RemoteMaintenanceFileName set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.SystemProtection == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No SystemProtection set)");
                    Logger.Log("ERROR: Configuation file invalid. (No SystemProtection set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (Core.Configuration.SPMaxMemoryDump == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No SPMaxMemoryDump set)");
                    Logger.Log("ERROR: Configuation file invalid. (No SPMaxMemoryDump set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
            }

            private static void playSound(string Filepath)
            {
                using var ms = File.OpenRead(Filepath);
                using var rdr = new Mp3FileReader(ms);
                using var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr);
                using var baStream = new BlockAlignReductionStream(wavStream);
                using var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
                waveOut.Init(baStream);
                waveOut.Play();
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }

            public static void Copy(string sourceDir, string targetDir)
            {
                Directory.CreateDirectory(targetDir);

                foreach (var file in Directory.GetFiles(sourceDir))
                    File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);

                foreach (var directory in Directory.GetDirectories(sourceDir))
                    Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }

            public static void MainMenu()
            {
                int windows_counter = 0;

                using var writer = new HighSpeedWriter();
                var window = new Window(writer);

                window.CursorVisible = false;

                var left = window.SplitLeft();
                var leftConsoles = left.SplitRows(
                    new Split(0),
                    new Split(9, "Audit log"),
                    new Split(10)
                    );

                var status = leftConsoles[1];
                status.BackgroundColor = ConsoleColor.Yellow;
                status.ForegroundColor = ConsoleColor.Red;
                status.Clear();

                // Window definition ----------------------
                var infoCon = leftConsoles[0];
                var menuCon = leftConsoles[2];
                var statusCon = window.SplitRight("Status");
                // ----------------------------------------

                statusCon.WriteLine(ConsoleColor.Green, "[i] AutoInit is ready!");
                Logger.Log("AutoInit is ready!");

                var t1 = Task.Run(() => {
                    while (!finished)
                    {
                        if (switchToAdmin)
                        {
                            while (switchToAdmin && !finished)
                            {
                                int status;
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Switch to Administrator account ...");
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Enable Administrator account ...");
                                writer.Flush();

                                status = Core.Actions.SwitchToAdmin.EnableAdmin();
                                
                                if (status != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot enable Administrator account!");
                                    writer.Flush();
                                    switchToAdmin = false;
                                    break;
                                }


                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Change Administrator password ...");
                                writer.Flush();

                                status = Core.Actions.SwitchToAdmin.UpdateAdminPassword(Core.Configuration.AdminPassword);

                                if (status != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot change password from Administrator account!");
                                    writer.Flush();
                                    switchToAdmin = false;
                                    break;
                                }

                                if (Core.Configuration.RemoveDefaultUser)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Remove User account ...");
                                    writer.Flush();

                                    status = Core.Actions.SwitchToAdmin.RemoveUser(Core.Configuration.DefaultUsername);

                                    if (status != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot delete account '{Core.Configuration.DefaultUsername}'!");
                                        writer.Flush();
                                        switchToAdmin = false;
                                        break;
                                    }
                                }

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Switched to admin!");
                                switchToAdmin = false;
                                writer.Flush();
                            }
                        }
                        else if (removeBloadware)
                        {
                            while (removeBloadware && !finished)
                            {
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Remove Bloatware ...");
                                int statuscode;

                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing App Connector ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Appconnector");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");

                                // Initialize list of apps to remove
                                Core.Actions.AppxManagement.apps = new List<Core.Actions.AppxManagement.App>();
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "App Connector", ID = "Microsoft.Appconnector" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Get Help", ID = "Microsoft.GetHelp" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Tips app", ID = "Microsoft.Getstarted" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Messaging", ID = "Microsoft.Messaging" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "People", ID = "Microsoft.People" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Print3D", ID = "Microsoft.Print3D" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Mobile Plans", ID = "Microsoft.OneConnect" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "GroupMe", ID = "Microsoft.GroupMe10" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "3D Builder", ID = "Microsoft.3DBuilder" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "MSN Weather", ID = "Microsoft.BingWeather" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "MSN Sports", ID = "Microsoft.BingSports" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "MSN News", ID = "Microsoft.BingNews" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "MSN Finance", ID = "Microsoft.BingFinance" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Sway", ID = "Microsoft.Office.Sway" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox App", ID = "Microsoft.XboxApp" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "To Do app", ID = "Microsoft.Todos" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Flipboard", ID = "Flipboard.Flipboard" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Twitter", ID = "9E2F88E3.Twitter" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Your Phone", ID = "Microsoft.YourPhone" });
                                Core.Actions.AppxManagement.apps.Add(new Core.Actions.AppxManagement.App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop" });

                                int progress = 0;
                                foreach (var app in Core.Actions.AppxManagement.apps)
                                {
                                    progress++;
                                    statusCon.WriteLine(ConsoleColor.Yellow, $"   -> Removing {app.Name} ...");

                                    statuscode = Core.Actions.AppxManagement.RemoveAppx(app.ID);

                                    if (statuscode != 0) statusCon.WriteLine(ConsoleColor.Red, $"   -> Cannot remove {app.Name}! Error: {statuscode}");
                                }

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Bloatware removed!");
                                removeBloadware = false;
                                writer.Flush();
                            }
                        }
                        else if (installApplications)
                        {
                            while (installApplications && !finished)
                            {
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Install Applications ...");
                                writer.Flush();
                                Logger.Log("User requested to install applications.");

                                int status;

                                // Check if winget is installed on the device
                                bool isWingetInstalled = Core.Actions.AppxManagement.IsWingetInstalled();
                                if (!isWingetInstalled)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] Error: Winget not found. Please update App Installer!");
                                    writer.Flush();
                                    Logger.Log("Error: Winget not found. Please update App Installer!");

                                    installApplications = false;
                                    break;
                                }

                                // Begin installation
                                statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Applications with WinGet ...");
                                writer.Flush();
                                Logger.Log("Installing Applications with WinGet ...");

                                if (Core.Configuration.InstallFirefox)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Firefox ...");
                                    writer.Flush();
                                    Logger.Log("Installing Firefox ...");

                                    status = Core.Actions.AppxManagement.InstallApp(Core.Configuration.PackageID_Firefox);

                                    if (status != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Firefox cannot be installed. Error: {status}");
                                        writer.Flush();
                                        Logger.Log($"Firefox cannot be installed. Error: {status}");
                                    }
                                }

                                if (Core.Configuration.InstallAcrobatReader)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Adobe Acrobat Reader DC ...");
                                    writer.Flush();
                                    Logger.Log("Installing Adobe Acrobat Reader DC ...");

                                    status = Core.Actions.AppxManagement.InstallApp(Core.Configuration.PackageID_AcrobatReader);

                                    if (status != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Adobe Acrobat Reader DC cannot be installed. Error: {status}");
                                        writer.Flush();
                                        Logger.Log($"Adobe Acrobat Reader DC cannot be installed. Error: {status}");
                                    }
                                }

                                if (!String.IsNullOrEmpty(Core.Configuration.OtherApps))
                                {
                                    string[] otherapps = Core.Configuration.OtherApps.Split(',');
                                    foreach (string app in otherapps)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Yellow,$"    -> Application '{app}'");
                                        writer.Flush();
                                        Logger.Log($"Installing Application with PackageID '{app}' ...");

                                        status = Core.Actions.AppxManagement.InstallApp(app);

                                        if (status != 0)
                                        {
                                            statusCon.WriteLine(ConsoleColor.Red, $"[!] Application '{app}' cannot be installed. Error: {status}");
                                            writer.Flush();
                                            Logger.Log($"Application '{app}' cannot be installed. Error: {status}");
                                        }
                                    }
                                }

                                if (Core.Configuration.RemoteMaintenance)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Remote maintenance software ...");
                                    writer.Flush();

                                    string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                                    string rmsFn = Path.Combine(publicDesktop, Core.Configuration.RemoteMaintenanceFileName);
                                    Logger.Log("Remote maintenance software downloaded.");
                                    
                                    status = Core.Actions.AppxManagement.InstallRM(Core.Configuration.RemoteMaintenanceURL, rmsFn);

                                    if (status != 0) 
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot download Remote maintenance software! Error: {status}");
                                        Logger.Log("Cannot download Remote maintenance software!");
                                    }
                                }

                                if (Core.Configuration.EnableNET35)
                                {
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Windows Features ...");
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> .NET Framework 3.5 ...");
                                    writer.Flush();

                                    status = Core.Actions.AppxManagement.InstallFeature("NetFx3");

                                    switch (status)
                                    {
                                        case 0:
                                            break;

                                        case 3010: // 3010 = ERROR_SUCCESS_REBOOT_REQUIRED
                                            statusCon.WriteLine(ConsoleColor.Yellow, $"[!] .NET Framework 3.5 was installed but a reboot is required!");
                                            writer.Flush();
                                            Logger.Log($".NET Framework 3.5 was installed but a reboot is required.");
                                            break;

                                        default:
                                            statusCon.WriteLine(ConsoleColor.Red, $"[!] .NET Framework 3.5 cannot be installed. Error: {status}");
                                            writer.Flush();
                                            Logger.Log($".NET Framework 3.5 cannot be installed. Error: {status}");
                                            break;
                                    }
                                }

                                if (Core.Configuration.EnableSMB1)
                                {
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Windows Features ...");
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> SMB 1 Protocol ...");
                                    writer.Flush();

                                    status = Core.Actions.AppxManagement.InstallFeature("SMB1Protocol");

                                    switch (status)
                                    {
                                        case 0:
                                            break;

                                        case 3010: // 3010 = ERROR_SUCCESS_REBOOT_REQUIRED
                                            statusCon.WriteLine(ConsoleColor.Yellow, $"[!] SMB 1 Protocol was installed but a reboot is required!");
                                            writer.Flush();
                                            Logger.Log($"SMB 1 Protocol was installed but a reboot is required.");
                                            break;

                                        default:
                                            statusCon.WriteLine(ConsoleColor.Red, $"[!] SMB 1 Protocol cannot be installed. Error: {status}");
                                            writer.Flush();
                                            Logger.Log($"SMB 1 Protocol cannot be installed. Error: {status}");
                                            break;
                                    }
                                }

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Applications installed!");
                                writer.Flush();
                                Logger.Log("Applications installed!");
                                installApplications = false;
                            }
                        }
                        else if (reinstallWindows)
                        {
                            while (reinstallWindows && !finished)
                            {
                                Logger.Log("User requested to reinstall Windows.");
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Reinstall Windows ...");
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Downloading recovery image ...");
                                writer.Flush();
                            }
                        }
                        else if (configureWindows)
                        {
                            while (configureWindows && !finished)
                            {
                                Logger.Log("User requested to configure Windows.");

                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Configure Windows Installation ...");

                                #region Telemetry
                                if (Core.Configuration.DisableTelemetry)
                                {
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Disable Telemetry ...");
                                    writer.Flush();
                                    Logger.Log("Disable Telemetry ...");

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Customer Experience Improvement (CEIP/SQM) ...");
                                    writer.Flush();
                                    Logger.Log("Customer Experience Improvement (CEIP/SQM) ...");

                                    if (!ConfigureWindows.DisableCEI())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] CEI cannot be disabled.");
                                        writer.Flush();
                                        Logger.Log("CEI cannot be disabled.");
                                    }

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Application Impact Telemetry (AIT) ...");
                                    writer.Flush();
                                    Logger.Log("Application Impact Telemetry (AIT) ...");

                                    if (!ConfigureWindows.DisableAIT())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] AIT cannot be disabled.");
                                        writer.Flush();
                                        Logger.Log("AIT cannot be disabled.");
                                    }

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Customer Experience Improvement Program ...");
                                    writer.Flush();
                                    Logger.Log("Customer Experience Improvement Program ...");

                                    if (!ConfigureWindows.DisableCEIP())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] CEIP cannot be disabled.");
                                        writer.Flush();
                                        Logger.Log("CEIP cannot be disabled.");
                                    }

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Telemetry in Data Collection Policy) ...");
                                    writer.Flush();
                                    Logger.Log("Telemetry in Data Collection Policy) ...");

                                    if (!ConfigureWindows.DisableDCP())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] DCP cannot be disabled.");
                                        writer.Flush();
                                        Logger.Log("DCP cannot be disabled.");
                                    }

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> License Telemetry ...");
                                    writer.Flush();
                                    Logger.Log("License Telemetry ...");

                                    if (!ConfigureWindows.DisableLicenseTel())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] TLT cannot be disabled. Error while writing into Registry.");
                                        writer.Flush();
                                        Logger.Log("TLT cannot be disabled. Error while writing into Registry.");
                                    }

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Devicesensus Telemetry Task...");
                                    writer.Flush();
                                    Logger.Log("Devicesensus Telemetry Task ...");

                                    if (!ConfigureWindows.DisableDeviceSensus())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] CEIP cannot be disabled. Error while writing into Registry.");
                                        writer.Flush();
                                        Logger.Log("Devicesensus Telemetry Task ...");
                                    }
                                }
                                #endregion

                                #region Windows Activation
                                if (Core.Configuration.CheckActivation)
                                {
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Checking system ...");
                                    writer.Flush();
                                    Logger.Log("Checking system ...");

                                    // ------------------------------------------------------------------------------------------------------------

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Check activation state ...");
                                    writer.Flush();
                                    Logger.Log("Check activation state ...");

                                    if (!ConfigureWindows.IsWindowsActivated())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Windows is not activated.");
                                        writer.Flush();
                                        Logger.Log("Windows is not activated.");
                                    }
                                }
                                #endregion

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Tweaking Windows ...");
                                writer.Flush();
                                Logger.Log("Tweaking Windows ...");

                                #region Disable auto reboot after BSOD
                                if (Core.Configuration.DisableAutoRebootAfterBSOD)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Disable auto reboot after BSOD ...");
                                    writer.Flush();
                                    Logger.Log("Disable auto reboot after BSOD ...");

                                    if (!ConfigureWindows.DisableAutoRebootOnBSOD())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Auto Reboot cannot be disabled.");
                                        writer.Flush();
                                        Logger.Log("Auto Reboot cannot be disabled.");
                                    }
                                }
                                #endregion

                                #region Maximum memory dump size at Kernel panic
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Set max. memory dump ...");
                                writer.Flush();
                                Logger.Log("Set max. memory dump to small ...");
                                
                                if (!ConfigureWindows.SetMaxMemDump())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set max. memory dump.");
                                    writer.Flush();
                                    Logger.Log("Cannot set max. memory dump.");
                                }
                                #endregion

                                #region System proection
                                statusCon.WriteLine(ConsoleColor.Yellow, $"    -> Enable System protection (max. Usage: {Core.Configuration.SystemProtection}) ...");
                                writer.Flush();
                                Logger.Log($"Enable System protection (max. Usage: {Core.Configuration.SystemProtection}) ...");
                                
                                if (!ConfigureWindows.ShadowStorage())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set max Usage of System protection to {Core.Configuration.SystemProtection}.");
                                    writer.Flush();
                                    Logger.Log($"Cannot set max Usage of System protection to {Core.Configuration.SystemProtection}.");
                                }
                                #endregion

                                #region Performance plan
                                if (Core.Configuration.SetMaxPerformance)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Set power plan to High Performance ...");
                                    writer.Flush();
                                    Logger.Log("Set power plan to High Performance ...");

                                    if (!ConfigureWindows.SetMaxPerformance())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set power plan to High Performance");
                                        writer.Flush();
                                        Logger.Log("Cannot set power plan to High Performance");
                                    }
                                }
                                #endregion

                                #region Fast Boot
                                if (Core.Configuration.DisableFastBoot)
                                {
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Disable Fast Boot ...");
                                    writer.Flush();
                                    Logger.Log("Disable Fast Boot ...");

                                    if (!ConfigureWindows.DisableFastBoot())
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot disable Fast Boot.");
                                        writer.Flush();
                                        Logger.Log("Cannot disable Fast Boot.");
                                    }
                                }
                                #endregion

                                writer.Flush();
                            }
                        }
                        else
                        {
                            writer.Flush();
                        }
                    }
                });

                var t3 = Task.Run(() =>
                {
                    var info = infoCon.SplitLeft("Information");
                    var matrix = infoCon.SplitRight("Skull");

                    info.WriteLine($"\nAdministrator Password:\n{Core.Configuration.AdminPassword}\n\nDefault Username:\n{Core.Configuration.DefaultUsername}\n\nPress up and down to select.\nPress ESC to exit.");
                    matrix.ForegroundColor = ConsoleColor.Green;
                    matrix.WriteLine(@"");
                    matrix.WriteLine(@"     |             /  |  ");
                    matrix.WriteLine(@"     /__   Y  __  (  _/  ");
                    matrix.WriteLine(@"     \`--`-'-|`---\\/    ");
                    matrix.WriteLine(@"      |'__/   ` __/ |    ");
                    matrix.WriteLine(@"      '-.   w   ,--/     ");
                    matrix.WriteLine(@"        |'_._._/  /      ");
                    matrix.WriteLine(@"        |________/       ");

                });

                // create a menu inside the menu console window
                // the menu will write updates to the status console window

                var menu = new Menu(menuCon, "Menu", ConsoleKey.Escape, 56,
                    new MenuItem('a', "Switch to Administrator", () =>
                    {
                        switchToAdmin = true;
                        removeBloadware = false;
                        installApplications = false;
                        configureWindows = false;
                        reinstallWindows = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Switch to Administrator ");
                    }),
                    new MenuItem('r', "Remove Bloatware", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = true;
                        installApplications = false;
                        configureWindows = false;
                        reinstallWindows = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Remove Bloatware ");
                    }),
                    new MenuItem('i', "Install Applications", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = false;
                        installApplications = true;
                        configureWindows = false;
                        reinstallWindows = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Install Applications ");
                    }),
                    new MenuItem('c', "Configure Windows (WIP - not working)", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = false;
                        installApplications = false;
                        configureWindows = true;
                        reinstallWindows = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Configure Windows ");
                    }),
                    new MenuItem('W', "Reinstall Windows (WIP - not working)", () =>
                    {
                        windows_counter++;
                        if (windows_counter != 2)
                        {
                            statusCon.WriteLine(ConsoleColor.Red, "[!] WARNING! This operation will reinstall Windows.");
                            statusCon.WriteLine(ConsoleColor.Red, "    Press again if you want to perform this action!");
                        }
                        else
                        {
                            switchToAdmin = false;
                            removeBloadware = false;
                            installApplications = false;
                            configureWindows = false;
                            reinstallWindows = true;
                            status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                            status.WriteLine(ConsoleColor.Red, $" Reinstall Windows ");
                        }
                    })
                );

                // menu writes to the console automatically,
                // but because we're using a buffered screen writer
                // we need to flush the UI after any menu action.
                menu.OnAfterMenuItem = _ => writer.Flush();

                menu.Run();
                // menu will block until user presses the exit key.

                finished = true;
                Task.WaitAll(t1, t3);

                window.Clear();
                window.WriteLine("Thanks for using AutoInit!");
                writer.Flush();
            }
        }
    }
}