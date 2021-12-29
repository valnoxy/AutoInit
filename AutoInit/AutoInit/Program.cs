using Konsole;
using Konsole.Forms;
using Konsole.Internal;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
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
                        playSound(files[rand.Next(files.Length)]);
                    }
                });

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

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Extracting script ...");
                                writer.Flush();

                                string tempfile = Path.GetTempFileName();
                                try
                                {
                                    if (File.Exists(tempfile))
                                        File.Delete(tempfile);
                                    File.WriteAllText(tempfile, Scripts.RemoveBloatware);
                                    string newTempFile = Path.ChangeExtension(tempfile, ".bat");
                                    File.Move(tempfile, newTempFile);
                                    tempfile = newTempFile;
                                }
                                catch (Exception ex)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot extract script! Error: {ex}");
                                    writer.Flush();
                                    removeBloadware = false;
                                    break;
                                }

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Running script ...");
                                writer.Flush();
                                Process p = new Process();
                                p.StartInfo.FileName = "cmd.exe";
                                p.StartInfo.Arguments = $"/c {tempfile}";
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.CreateNoWindow = true;
                                p.Start();
                                p.WaitForExit();

                                if (p.ExitCode != 0)
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot run script! Error: {p.ExitCode}");
                                    writer.Flush();
                                    removeBloadware = false;
                                    break;
                                }

                                statusCon.WriteLine(ConsoleColor.Yellow, "    -> Cleaning up ...");
                                writer.Flush();

                                try
                                {
                                    File.Delete(tempfile);
                                }
                                catch
                                {
                                    statusCon.WriteLine(ConsoleColor.Red, $"[!] Cannot remove script! Error: {p.ExitCode}");
                                    writer.Flush();
                                    removeBloadware = false;
                                    break;
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
                                        WebClient rms = new WebClient();
                                        string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                                        string rmsFN = Path.Combine(publicDesktop, "Fernwartung Wolkenhof.exe");
                                        rms.DownloadFile(RemoteMaintenance, rmsFN) ;
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
                    matrix.WriteLine(@"     |             /  |");
                    matrix.WriteLine(@"     /__   Y  __  (  _/");
                    matrix.WriteLine(@"     \`--`-'-|`---\\/");
                    matrix.WriteLine(@"      |'__/   ` __/ |");
                    matrix.WriteLine(@"      '-.   w   ,--/");
                    matrix.WriteLine(@"        |'_._._/  /");
                    matrix.WriteLine(@"        |________/");
                    
                });

                // create a menu inside the menu console window
                // the menu will write updates to the status console window

                var menu = new Menu(menuCon, "Menu", ConsoleKey.Escape, 56,
                    new MenuItem('a', "Switch to Administrator", () =>
                    {
                        switchToAdmin = true;
                        removeBloadware = false;
                        installApplications = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Switch to Administrator ");
                    }),
                    new MenuItem('r', "Remove Bloatware", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = true;
                        installApplications = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Remove Bloatware ");
                    }),
                    new MenuItem('i', "Install Applications", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = false;
                        installApplications = true;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Install Applications ");
                    }),
                    new MenuItem('m', "Mute / Play Music", () =>
                    {
                        switchToAdmin = false;
                        removeBloadware = false;
                        installApplications = false;
                        status.Write(ConsoleColor.White, $"{Environment.UserName} : {DateTime.Now.ToString("HH:mm:ss -")}");
                        status.WriteLine(ConsoleColor.Red, $" Mute / Play Music ");
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