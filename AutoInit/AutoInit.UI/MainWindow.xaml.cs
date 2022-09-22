using AutoInit.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoInit.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private BackgroundWorker bWSwitch = new BackgroundWorker();
        private BackgroundWorker bWBloatware = new BackgroundWorker();
        private BackgroundWorker bWSoftware = new BackgroundWorker();
        private string AdminPassword;

        public MainWindow()
        {
            InitializeComponent();

            if (TbPassword.Password.Length > 0)
            {
                SwitchActionCard.IsEnabled = true;
            }
            else
            {
                SwitchActionCard.IsEnabled = false;
            }
            // bWSwitch
            bWSwitch.WorkerReportsProgress = true;
            bWSwitch.WorkerSupportsCancellation = true;
            bWSwitch.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bWSwitch.DoWork += new DoWorkEventHandler(SwitchToAdmin_DoWork);
            // bWBloatware
            bWBloatware.WorkerReportsProgress = true;
            bWBloatware.WorkerSupportsCancellation = true;
            bWBloatware.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bWBloatware.DoWork += new DoWorkEventHandler(RemoveBloatware_DoWork);
            // bWSoftware
            bWSoftware.WorkerReportsProgress = true;
            bWSoftware.WorkerSupportsCancellation = true;
            bWSoftware.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bWSoftware.DoWork += new DoWorkEventHandler(InstallSoftware_DoWork);
        }

        public void SwitchToAdmin_Click(object sender, RoutedEventArgs e) => bWSwitch.RunWorkerAsync();
        public void RemoveBloatware_Click(object sender, RoutedEventArgs e) => bWBloatware.RunWorkerAsync();
        public void InstallActionCard_Click(object sender, RoutedEventArgs e) => bWSoftware.RunWorkerAsync();
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) => ProgrBar.Value = e.ProgressPercentage;

        void SwitchToAdmin_DoWork(object sender, DoWorkEventArgs e)
        {
            LockUI();

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(0);

            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Change Administrator password ...";
            }), DispatcherPriority.ContextIdle);

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c \"net user Administrator {AdminPassword}\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                bWSwitch.ReportProgress(0);
                Dispatcher.Invoke(new Action(() => {
                    ActionLabel.Content = $"Cannot change password from Administrator account! Error: {p.ExitCode}";
                }), DispatcherPriority.ContextIdle);
                UnlockUI();
                bWSwitch.CancelAsync();

                return;
            }

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(33);
            
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Enable Administrator account ...";
            }), DispatcherPriority.ContextIdle);

            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c \"net user Administrator /active:yes\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                bWSwitch.ReportProgress(0);

                if (p.ExitCode == 2)
                {
                    Dispatcher.Invoke(
                        new Action(() =>
                        {
                            ActionLabel.Content =
                                $"Cannot enable Administrator account! The password does not meet the password policy requirements.";
                        }), DispatcherPriority.ContextIdle);
                }
                else
                {
                    Dispatcher.Invoke(
                        new Action(() =>
                        {
                            ActionLabel.Content = $"Cannot enable Administrator account! Error: {p.ExitCode}";
                        }), DispatcherPriority.ContextIdle);
                }
                UnlockUI();
                bWSwitch.CancelAsync();
                
                return;
            }

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(66);
            
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Remove account 'User' ...";
            }), DispatcherPriority.ContextIdle);
            
            p.StartInfo.Arguments = $"/c \"net user User /delete\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                bWSwitch.ReportProgress(0);
                
                Dispatcher.Invoke(new Action(() => {
                    ActionLabel.Content = $"Cannot delete account 'User'! Error: {p.ExitCode}";
                }), DispatcherPriority.ContextIdle);
                UnlockUI();
                bWSwitch.CancelAsync();

                return;
            }

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(100);
            
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Successfully switched to Administrator. Please log out now.";
            }), DispatcherPriority.ContextIdle);
            UnlockUI();
        }

        void RemoveBloatware_DoWork(object sender, DoWorkEventArgs e)
        {
            LockUI();
            int statuscode;
            bWBloatware.ReportProgress(0);
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Removing bloatware ...";
            }), DispatcherPriority.ContextIdle);

            // Initialize list of apps to remove
            AppxRemove.apps = new List<AppxRemove.App>();
            AppxRemove.apps.Add(new AppxRemove.App { Name = "App Connector", ID = "Microsoft.Appconnector" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Get Help", ID = "Microsoft.GetHelp" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Messaging", ID = "Microsoft.Messaging" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "People", ID = "Microsoft.People" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Print3D", ID = "Microsoft.Print3D" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Mobile Plans", ID = "Microsoft.OneConnect" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "GroupMe", ID = "Microsoft.GroupMe10" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "3D Builder", ID = "Microsoft.3DBuilder" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "MSN Weather", ID = "Microsoft.BingWeather" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "MSN Sports", ID = "Microsoft.BingSports" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "MSN News", ID = "Microsoft.BingNews" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "MSN Finance", ID = "Microsoft.BingFinance" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Sway", ID = "Microsoft.Office.Sway" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox App", ID = "Microsoft.XboxApp" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "To Do app", ID = "Microsoft.Todos" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Flipboard", ID = "Flipboard.Flipboard" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Twitter", ID = "9E2F88E3.Twitter" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Your Phone", ID = "Microsoft.YourPhone" });
            AppxRemove.apps.Add(new AppxRemove.App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop" });

            // foreach app in list, remove app with appxremove and add to progress bar
            int progress = 0;
            foreach (var app in AppxRemove.apps)
            {
                progress++;
                Dispatcher.Invoke(new Action(() => {
                    ActionLabel.Content = $"Removing {app.Name} ...";
                }), DispatcherPriority.ContextIdle);
                
                statuscode = AppxRemove.RemoveAppx(app.ID);
                
                if (statuscode != 0)
                {
                    Dispatcher.Invoke(new Action(() => {
                        ActionLabel.Content = $"Cannot remove {app.Name}! Error: {statuscode}";
                        ProgrBar.Value = 0;
                    }), DispatcherPriority.ContextIdle);
                }

                double progressValue1 = (double)progress / (double)AppxRemove.apps.Count;
                double progressValue2 = progressValue1 * 100;
                int progressValue = Convert.ToInt32(progressValue2);

                bWBloatware.ReportProgress(progressValue);
            }

            bWBloatware.ReportProgress(100);
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = "Bloatware removed!";
            }), DispatcherPriority.ContextIdle);
            UnlockUI();
        }

        private void InstallSoftware_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                Dispatcher.Invoke(new Action(() => {
                    ProgrBar.Value = i;
                }), DispatcherPriority.ContextIdle);
                Thread.Sleep(100);
            }

        }

        private void LockUI()
        {
            Dispatcher.Invoke(new Action(() => {
                TbPassword.IsEnabled = false;
                SwitchActionCard.IsEnabled = false;
                RemoveActionCard.IsEnabled = false;
                InstallActionCard.IsEnabled = false;
                ConfigureActionCard.IsEnabled = false;
                ReinstallActionCard.IsEnabled = false;
            }), DispatcherPriority.ContextIdle);
        }

        private void UnlockUI()
        {
            Dispatcher.Invoke(new Action(() => {
                TbPassword.IsEnabled = true;
                if (TbPassword.Password.Length > 0)
                {
                    SwitchActionCard.IsEnabled = true;
                }
                else
                {
                    SwitchActionCard.IsEnabled = false;
                }
                RemoveActionCard.IsEnabled = true;
                InstallActionCard.IsEnabled = true;
                ConfigureActionCard.IsEnabled = true;
                ReinstallActionCard.IsEnabled = true;
            }), DispatcherPriority.ContextIdle);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TbPassword_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbPassword.Password.Length > 0)
            {
                SwitchActionCard.IsEnabled = true;
                AdminPassword = TbPassword.Password;
            }
            else
            {
                SwitchActionCard.IsEnabled = false;
            }
        }
    }
}
