using Konsole;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AutoInit
{
    internal class Program
    {
        public static class AutoInit
        {
            static bool finished = false;
            static bool switchToAdmin = false;
            static bool removeBloadware = false;
            static bool installApplications = false;
            static bool configureWindows = false;
            static bool reinstallWindows = false;

            static string MusicDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music");

            static string Config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
            static string AdminPW;
            static string RemoteMaintenance;
            static string PackageID_Firefox;
            static string PackageID_AcrobatReader;
            static string DotNet;
            static string SMB;

            public static void Main(string[] args)
            {
                // Configuation check
                if (!File.Exists(Config))
                {
                    Console.WriteLine("ERROR: No configuation file found. (File missing: config.ini)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                var ConfigIni = new IniFile(Config);
                AdminPW = ConfigIni.Read("AdminPW");
                RemoteMaintenance = ConfigIni.Read("RemoteMaintenance");
                PackageID_Firefox = ConfigIni.Read("PackageID_Firefox");
                PackageID_AcrobatReader = ConfigIni.Read("PackageID_AcrobatReader");
                DotNet = ConfigIni.Read("DotNet");
                SMB = ConfigIni.Read("SMB");

                if (AdminPW == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No AdminPW set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (RemoteMaintenance == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No RemoteMaintenance set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (PackageID_Firefox == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No PackageID_Firefox set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (PackageID_AcrobatReader == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No PackageID_AcrobatReader set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (DotNet == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No DotNet set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }
                if (SMB == "")
                {
                    Console.WriteLine("ERROR: Configuation file invalid. (No SMB set)");
                    Thread.Sleep(3);
                    Environment.Exit(1);
                }

                var music = Task.Run(() =>
                {
                    if (Directory.Exists(MusicDir))
                    {
                        var rand = new Random();
                        var files = Directory.GetFiles(MusicDir, "*.mp3");
                        try
                        {
                            playSound(files[rand.Next(files.Length)]);
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[!] Cannot play audio... skipping...");
                            Thread.Sleep(2000);
                            Console.ResetColor();
                        }
                    }
                });

                // Check args
                if (args != null || args.Length != 0)
                {
                    if (args.Length == 1)
                    {
                        string arg = args[0];
                        if (args[0] == "--no-intro")
                        {
                            Entry();
                            Environment.Exit(0);
                        }
                    }
                }

                Intro.StartIntro();
                Entry();
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

            public static void Entry()
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

                var t1 = Task.Run(() => {
                    while (!finished)
                    {
                        if (switchToAdmin)
                        {
                            while (switchToAdmin && !finished)
                            {
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Switch to Administrator account ...");
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Enable Administrator account ...");
                                writer.Flush();
                                Process p = new Process();
                                p.StartInfo.FileName = "cmd.exe";
                                p.StartInfo.Arguments = "/c \"net user Administrator /active:yes\"";
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.CreateNoWindow = true;
                                p.Start();
                                p.WaitForExit();

                                if (p.ExitCode != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot enable Administrator account! Error: {p.ExitCode}");
                                    writer.Flush();
                                    switchToAdmin = false;
                                    break;
                                }


                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Change Administrator password ...");
                                writer.Flush();
                                p.StartInfo.Arguments = $"/c \"net user Administrator {AdminPW}\"";
                                p.Start();
                                p.WaitForExit();

                                if (p.ExitCode != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot change password from Administrator account! Error: {p.ExitCode}");
                                    writer.Flush();
                                    switchToAdmin = false;
                                    break;
                                }

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Remove User account ...");
                                writer.Flush();
                                p.StartInfo.Arguments = $"/c \"net user User /delete\"";
                                p.Start();
                                p.WaitForExit();

                                if (p.ExitCode != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot delete User account! Error: {p.ExitCode}");
                                    writer.Flush();
                                    switchToAdmin = false;
                                    break;
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
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Cortana ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.549981C3F5F10");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Get Help ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.GetHelp");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Tips app ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Getstarted");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Messaging ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Messaging");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Mixed Reality Portal ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.MixedReality.Portal");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Windows Feedback Hub ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsFeedbackHub");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Windows Alarms ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsAlarms");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Windows Camera ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsCamera");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Windows Maps ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsMaps");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Minecraft for Windows 10 Edition ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.MinecraftUWP");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing People ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.People");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Print3D...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Print3D");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Mobile Plans ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.OneConnect");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Microsoft Solitaire Collection ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftSolitaireCollection");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Sticky Notes ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftStickyNotes");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing GroupMe ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.GroupMe10");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Voice Recorder ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsSoundRecorder");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing 3D Builder ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.3DBuilder");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing 3D Viewer ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Microsoft3DViewer");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing MSN Weather ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.BingWeather");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing MSN Sports ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.BingSports");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing MSN News ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.BingNews");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing MSN Finance ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.BingFinance");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing My Office ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftOfficeHub");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Office OneNote ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Office.OneNote");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Sway ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Office.Sway");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox App ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.XboxApp");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox Live in-game experience ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Xbox.TCUI");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox Game Bar ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.XboxGamingOverlay");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox Game Bar Plugin ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.XboxGameOverlay");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox Identity Provider ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.XboxIdentityProvider");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Xbox Speech to Text Overlay ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.XboxSpeechToTextOverlay");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Network Speedtest ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.NetworkSpeedTest");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing To Do app ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Todos");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Shazam ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("ShazamEntertainmentLtd.Shazam");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Candy Crush ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("king.com.CandyCrushSaga");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                statuscode = AppxRemove.RemoveAppx("king.com.CandyCrushSodaSaga");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Flipboard ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Flipboard.Flipboard");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Twitter ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("9E2F88E3.Twitter");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing iHeartRadio ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("ClearChannelRadioDigital.iHeartRadio");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Duolingo ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("D5EA27B7.Duolingo-LearnLanguagesforFree");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Photoshop Express ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("AdobeSystemIncorporated.AdobePhotoshop");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Pandora ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("PandoraMediaInc.29680B314EFC2");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Eclipse Manager ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("46928bounde.EclipseManager");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Code Writer ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("ActiproSoftwareLLC.562882FEEB491");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Spotify ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("SpotifyAB.SpotifyMusic");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Your Phone Companion ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsPhone");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                statuscode = AppxRemove.RemoveAppx("Microsoft.Windows.Phone");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Communications - Phone app ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.CommsPhone");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Your Phone ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.YourPhone");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Removing Remote Desktop app ...");
                                writer.Flush();
                                statuscode = AppxRemove.RemoveAppx("Microsoft.RemoteDesktop");
                                if (statuscode != 0)
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] App cannot be removed!");
                                // ---------------------------------------------------------------------------

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
                                Process p = new Process();
                                p.StartInfo.FileName = "cmd.exe";
                                p.StartInfo.Arguments = "/c winget";
                                p.Start();
                                p.WaitForExit();

                                if (p.ExitCode != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, "[!] Error: Winget not found. Please update App Installer!");
                                    writer.Flush();
                                    installApplications = false;
                                    break;
                                }
                                else
                                {

                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Applications with WinGet ...");
                                    writer.Flush();

                                    p.StartInfo.Arguments = "/c winget list";
                                    p.Start();
                                    p.WaitForExit();
                                    p.StartInfo.UseShellExecute = false;
                                    p.StartInfo.CreateNoWindow = true;

                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Firefox ...");
                                    writer.Flush();
                                    p.StartInfo.Arguments = $"/c winget install --id {PackageID_Firefox}";
                                    p.Start();
                                    p.WaitForExit();

                                    if (p.ExitCode != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Firefox cannot be installed. Error: {p.ExitCode}");
                                        writer.Flush();
                                    }

                                    // ----------------------------------------------------------------------------
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Adobe Acrobat Reader DC ...");
                                    writer.Flush();
                                    p.StartInfo.Arguments = $"/c winget install --id {PackageID_AcrobatReader}";
                                    p.Start();
                                    p.WaitForExit();

                                    if (p.ExitCode != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Adobe Acrobat Reader DC cannot be installed. Error: {p.ExitCode}");
                                        writer.Flush();
                                    }

                                    // ----------------------------------------------------------------------------
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> Remote maintenance software ...");
                                    writer.Flush();
                                    try
                                    {
                                        WebClient rms = new();
                                        string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                                        string rmsFN = Path.Combine(publicDesktop, "Fernwartung Wolkenhof.exe");
                                        rms.DownloadFile(RemoteMaintenance, rmsFN);
                                    }
                                    catch
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot download Remote maintenance software! Error: {p.ExitCode}");
                                    }

                                    // ----------------------------------------------------------------------------
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Windows Features ...");
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> .NET Framework 3.5 ...");
                                    writer.Flush();
                                    p.StartInfo.Arguments = $"/c \"{DotNet}\"";
                                    p.Start();
                                    p.WaitForExit();

                                    if (p.ExitCode != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] .NET Framework 3.5 cannot be installed. Error: {p.ExitCode}");
                                        writer.Flush();
                                    }

                                    // ----------------------------------------------------------------------------
                                    statusCon.WriteLine(ConsoleColor.Green, "[i] Installing Windows Features ...");
                                    statusCon.WriteLine(ConsoleColor.Yellow, "    -> SMB 1 Protocol ...");
                                    writer.Flush();
                                    p.StartInfo.Arguments = $"/c \"{SMB}\"";
                                    p.Start();
                                    p.WaitForExit();

                                    if (p.ExitCode != 0)
                                    {
                                        statusCon.WriteLine(ConsoleColor.Red, $"[!] SMB 1 Protocol cannot be installed. Error: {p.ExitCode}");
                                        writer.Flush();
                                    }
                                }
                                statusCon.WriteLine(ConsoleColor.Green, "[i] Applications installed!");
                                installApplications = false;
                                writer.Flush();
                            }
                        }
                        else if (reinstallWindows)
                        {
                            while (reinstallWindows && !finished)
                            {
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Reinstall Windows ...");
                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Downloading recovery image ...");
                                writer.Flush();
                            }
                        }
                        else if (configureWindows)
                        {
                            while (configureWindows && !finished)
                            {
                                statusCon.WriteLine(ConsoleColor.Cyan, "[i] Configure Windows Installation ...");

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Disable Telemetry ...");

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Customer Experience Improvement (CEIP/SQM) ...");
                                if (!ConfigureWindows.DisableCEI())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] CEI cannot be disabled.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Application Impact Telemetry (AIT) ...");
                                if (!ConfigureWindows.DisableAIT())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] AIT cannot be disabled.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Customer Experience Improvement Program ...");
                                if (!ConfigureWindows.DisableCEIP())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] CEIP cannot be disabled.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Telemetry in Data Collection Policy) ...");
                                if (!ConfigureWindows.DisableDCP())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] DCP cannot be disabled.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> License Telemetry ...");
                                if (!ConfigureWindows.DisableLicenseTel())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] TLT cannot be disabled. Error while writing into Registry.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Devicesensus Telemetry Task...");
                                if (!ConfigureWindows.DisableDeviceSensus())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] CEIP cannot be disabled. Error while writing into Registry.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Checking system ...");

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Check activation state ...");
                                if (!ConfigureWindows.IsWindowsActivated())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Windows is not activated.");
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Green, "[i] Tweaking Windows ...");

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Disable auto reboot after BSOD ...");
                                if (!ConfigureWindows.DisableAutoRebootOnBSOD())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Auto Reboot cannot be disabled.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Set max. memory dump to small ...");
                                if (!ConfigureWindows.SetMaxMemDump())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set max. memory dump to small.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Enable System protection (max. Usage: 20%) ...");
                                if (!ConfigureWindows.ShadowStorage())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set max Usage of System protection to 20%.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Set power plan to High Performance ...");
                                if (!ConfigureWindows.SetMaxPerformance())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot set power plan to High Performance");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Disable Fast Boot ...");
                                if (!ConfigureWindows.DisableFastBoot())
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot disable Fast Boot.");
                                    writer.Flush();
                                }

                                // ------------------------------------------------------------------------------------------------------------

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

                    info.WriteLine($"\nAdministrator Password:\n{AdminPW}\n\n\n\nPress up and down to select.\nPress ESC to exit.");
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