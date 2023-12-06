using AutoInit.Boot.Common;
using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using AutoInit.Boot.Resources;
using Path = System.IO.Path;
using System.Security.Policy;
using XamlAnimatedGif;

namespace AutoInit.Boot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _applyBackgroundWorker;
        private string _currentAction;
        
        public MainWindow()
        {
            InitializeComponent();

            // Start logging
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var appVer = appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build + "." + appVersion.Revision;
            Core.Logger.StartLogging("Boot", appVer);


            _applyBackgroundWorker = new BackgroundWorker();
            _applyBackgroundWorker.WorkerReportsProgress = true;
            _applyBackgroundWorker.WorkerSupportsCancellation = true;
            //_applyBackgroundWorker.DoWork += CounterProgr;
            _applyBackgroundWorker.ProgressChanged += applyBackgroundWorker_ProgressChanged;
            _applyBackgroundWorker.RunWorkerAsync();
        }

        private bool _easterEggIsEnabled = false;

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.F) return;
            if (!_easterEggIsEnabled)
            {
                // Define gif
                var random = new Random();
                var randomValue = random.Next(1, 11);

                AnimationBehavior.SetSourceUri(EasterEgg,
                    randomValue == 1
                        ? new Uri("pack://application:,,,/Fadi2.gif")
                        : new Uri("pack://application:,,,/Fadi_Loop_f.gif"));

                EasterEgg.Visibility = Visibility.Visible;
                ProgressRing.Visibility = Visibility.Hidden;
                _easterEggIsEnabled = true;
            }
            else
            {
                EasterEgg.Visibility = Visibility.Hidden;
                ProgressRing.Visibility = Visibility.Visible;
                _easterEggIsEnabled = false;
            }
        }

        private void applyBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 100)
            {
                if (!string.IsNullOrEmpty((string?)e.UserState))
                {
                    TxtStatus.Text = $"{e.UserState} {e.ProgressPercentage}%\nBitte lassen Sie Ihren PC eingeschaltet.";
                    _currentAction = (string)e.UserState;
                }
                else
                {
                    TxtStatus.Text = $"{_currentAction} {e.ProgressPercentage}%\nBitte lassen Sie Ihren PC eingeschaltet.";
                }
            }
            else
            {
                switch (e.ProgressPercentage)
                {
                    case 101:
                        return;
                }
            }
        }

        private void CounterProgr(object? sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker?.ReportProgress(0, "Telemetrieanwendungen werden deaktiviert");
            Core.Logger.Log("Begin disabling of telemetry collection");

            // Disable telemetry (DCP)
            Core.Logger.Log("Telemetry DCP ...");
            Common.Registry.SetReg(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection", RegistryValueKind.DWord, 0);
            Common.Registry.SetReg(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", RegistryValueKind.DWord, 0);
            Common.Registry.SetReg(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", RegistryValueKind.DWord, 0);
            Common.Registry.SetReg(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", RegistryValueKind.DWord, 0);
            Common.Registry.SetReg(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", RegistryValueKind.DWord, 0);

            // CEIP / SQM
            Core.Logger.Log("Telemetry CEIP / SQM ...");
            worker?.ReportProgress(10, "");
            Common.Registry.SetReg(@"Software\Policies\Microsoft\SQMClient\Windows", RegistryValueKind.DWord, 0);

            // Remove Bloatware
            Core.Logger.Log("Begin removal of bloatware");
            Core.Logger.Log("Building Application list ...");
            AppxManagement.apps = new List<AppxManagement.App>();
            AppxManagement.apps.Add(new AppxManagement.App { Name = "3D Builder", ID = "Microsoft.3DBuilder" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "App Connector", ID = "Microsoft.Appconnector" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Flipboard", ID = "Flipboard.Flipboard" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Get Help", ID = "Microsoft.GetHelp" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "GroupMe", ID = "Microsoft.GroupMe10" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Messaging", ID = "Microsoft.Messaging" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Mobile Plans", ID = "Microsoft.OneConnect" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Finance", ID = "Microsoft.BingFinance" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN News", ID = "Microsoft.BingNews" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Sports", ID = "Microsoft.BingSports" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Weather", ID = "Microsoft.BingWeather" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "People", ID = "Microsoft.People" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Print3D", ID = "Microsoft.Print3D" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Sway", ID = "Microsoft.Office.Sway" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "To Do app", ID = "Microsoft.Todos" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Twitter", ID = "9E2F88E3.Twitter" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox App", ID = "Microsoft.XboxApp" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone", ID = "Microsoft.YourPhone" });

            foreach (var app in AppxManagement.apps)
            {
                Core.Logger.Log($"Removing application provision package '{app.Name}' with ID '{app.ID}'");
                worker?.ReportProgress(20, $"Bereitstellungspaket der App \"{app.Name}\" wird entfernt");
                try
                {
                    AppxManagement.RemoveAppXProvisionedPackage(app.ID);
                }
                catch (Exception ex)
                {
                    Core.Logger.Log($"Failed to remove app '{app.Name}' with ID '{app.ID}': {ex.Message}");
                }
            }

            var build = Environment.OSVersion.Version.Build;
            if (build >= 22000) // Windows 11+
            {
                worker?.ReportProgress(30, $"Bereinigung des Startmenüs");
                Core.Logger.Log("Replacing start2.bin with clean version ...");

                var root = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Users");
                var subdirectoriesEntries = Directory.GetDirectories(root);
                foreach (var subdirectory in subdirectoriesEntries)
                {
                    var startmenu = Path.Combine(subdirectory,
                        "AppData", "Local", "Packages", "Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy", "LocalState");
                    if (!Directory.Exists(startmenu)) continue;
                    try
                    {
                        Core.Logger.Log($"Writing binary file 'start2.bin' to '{startmenu}' ...");
                        File.WriteAllBytes(Path.Combine(startmenu, "start2.bin"), StartMenu.start2);
                    }
                    catch (Exception ex)
                    {
                        Core.Logger.Log("[ERROR] Failed to write binary file: " + ex.Message);
                    }
                }
                Environment.Exit(0);
            }

            // System Protection enabled + 20%
            // Show File Extension
            // Energy Option: Höchstleistung
            //                Fast Boot disabled

            #region Disable Auto Restar on BSoD

            worker?.ReportProgress(40, "Deaktivierung des automatischen Neustarts beim BSoD");
            try
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c \"wmic RecoverOS set AutoReboot = False\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };
                p.Start();
                p.WaitForExit();

                if (p.ExitCode != 0)
                    Core.Logger.Log("Failed to disable Auto Reboot on BSoD: " + p.ExitCode);

                worker?.ReportProgress(50, $"Anpassen der maximalen Speicherplatzbelegung auf C:\\");
                var p1 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c \"vssadmin resize shadowstorage /for=C: /on=C: /maxsize=20%\r\n\r\n\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };
                p1.Start();
                p1.WaitForExit();

                if (p1.ExitCode != 0)
                    Core.Logger.Log("Failed to disable Auto Reboot on BSoD: " + p1.ExitCode);
            }
            catch (Exception ex)
            {
                Core.Logger.Log("Failed to disable Auto Reboot on BSoD: " + ex.Message);
            }

            #endregion

            #region System Protection enabled + 20%

            worker?.ReportProgress(60, $"Aktivierung des Computerschutz auf Datenträger C:\\");
            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                CreateNoWindow = false,
                Arguments = $"Enable-ComputerRestore -Drive \"C:\\\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "powershell.exe"
            };
            var proc = Process.Start(psi);
            proc?.WaitForExit();

            worker?.ReportProgress(70, $"Anpassen der maximalen Speicherplatzbelegung auf C:\\");
            var p2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c \"vssadmin resize shadowstorage /for=C: /on=C: /maxsize=20%\r\n\r\n\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
            };
            p2.Start();
            p2.WaitForExit();

            if (p2.ExitCode != 0)
                Core.Logger.Log("Failed to disable Auto Reboot on BSoD: " + p2.ExitCode);

            #endregion
        }
    }
}
