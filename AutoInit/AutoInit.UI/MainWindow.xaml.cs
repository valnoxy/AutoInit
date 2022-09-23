using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AutoInit.Core.Actions;

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

        #region Worker

        void SwitchToAdmin_DoWork(object sender, DoWorkEventArgs e)
        {
            LockUI();

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(0);

            // Define status code integer
            int status;

            ReportAction("Change Administrator password ...");

            status = Core.Actions.SwitchToAdmin.UpdateAdminPassword(TbPassword.Password);

            if (status != 0)
            {
                bWSwitch.ReportProgress(0);
                ReportAction($"Cannot change password from Administrator account! Error: {status}");
                UnlockUI();
                bWSwitch.CancelAsync();

                return;
            }

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(33);
            ReportAction("Enable Administrator account ...");
            
            status = Core.Actions.SwitchToAdmin.EnableAdmin();

            if (status != 0)
            {
                bWSwitch.ReportProgress(0);

                ReportAction(status == 2
                    ? "Cannot enable Administrator account! The password does not meet the password policy requirements." // Policy error
                    : $"Cannot enable Administrator account! Error: {status}"); // Unknown error
                UnlockUI();
                bWSwitch.CancelAsync();
                
                return;
            }

            // Call event handler to update progress bar
            bWSwitch.ReportProgress(66);
            ReportAction("Remove account 'User' ...");
            
            status = Core.Actions.SwitchToAdmin.RemoveUser("User");

            if (status != 0)
            {
                bWSwitch.ReportProgress(0);
                ReportAction($"Cannot delete account 'User'! Error: {status}");
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
            ReportAction("Removing bloatware ...");

            // Initialize list of apps to remove
            AppxManagement.apps = new List<AppxManagement.App>();
            AppxManagement.apps.Add(new AppxManagement.App { Name = "App Connector", ID = "Microsoft.Appconnector" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Get Help", ID = "Microsoft.GetHelp" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Messaging", ID = "Microsoft.Messaging" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "People", ID = "Microsoft.People" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Print3D", ID = "Microsoft.Print3D" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Mobile Plans", ID = "Microsoft.OneConnect" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "GroupMe", ID = "Microsoft.GroupMe10" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "3D Builder", ID = "Microsoft.3DBuilder" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Weather", ID = "Microsoft.BingWeather" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Sports", ID = "Microsoft.BingSports" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN News", ID = "Microsoft.BingNews" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "MSN Finance", ID = "Microsoft.BingFinance" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Sway", ID = "Microsoft.Office.Sway" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox App", ID = "Microsoft.XboxApp" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "To Do app", ID = "Microsoft.Todos" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Flipboard", ID = "Flipboard.Flipboard" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Twitter", ID = "9E2F88E3.Twitter" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Your Phone", ID = "Microsoft.YourPhone" });
            AppxManagement.apps.Add(new AppxManagement.App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop" });

            int progress = 0;
            foreach (var app in AppxManagement.apps)
            {
                progress++;
                ReportAction($"Removing {app.Name} ...");
                
                statuscode = AppxManagement.RemoveAppx(app.ID);
                
                if (statuscode != 0) ReportAction($"Cannot remove {app.Name}! Error: {statuscode}");
                
                // Calculate progress bar content
                double progressValue1 = (double)progress / (double)AppxManagement.apps.Count;
                double progressValue2 = progressValue1 * 100;
                int progressValue = Convert.ToInt32(progressValue2);

                bWBloatware.ReportProgress(progressValue);
            }

            bWBloatware.ReportProgress(100);
            ReportAction("Bloatware removed!");
            UnlockUI();
        }

        private void InstallSoftware_DoWork(object sender, DoWorkEventArgs e)
        {
            LockUI();

            ReportAction("Installing software...");
            bWSoftware.ReportProgress(0);
            int status;

            // Check if winget is installed on the device
            bool isWingetInstalled = Core.Actions.AppxManagement.IsWingetInstalled();
            if (!isWingetInstalled)
            {
                ReportAction("Error: Winget not found. Please update App Installer!");
                UnlockUI();
                bWSwitch.CancelAsync();
                return;
            }
            bWSoftware.ReportProgress(20);

            // Firefox
            ReportAction("Installing Firefox ...");
            status = Core.Actions.AppxManagement.InstallApp("Mozilla.Firefox");
            if (status != 0)
            {
                ReportAction($"Cannot install 'Firefox'. Error: {status}");
            }
            bWSoftware.ReportProgress(40);

            // Adobe Acrobat Reader DC
            ReportAction("Installing Adobe Acrobat Reader DC ...");
            status = Core.Actions.AppxManagement.InstallApp("Adobe.Acrobat.Reader.64-bit");
            if (status != 0)
            {
                ReportAction($"Cannot install 'Adobe Acrobat Reader DC'. Error: {status}");
            }
            bWSoftware.ReportProgress(60);

            // Remote Maintenance Tool
            ReportAction("Installing Remote Maintenance Tool ...");
            string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            string rmsFn = Path.Combine(publicDesktop, "Fernwartung Wolkenhof.exe");
            status = Core.Actions.AppxManagement.InstallRM("https://wolkenhof.com/download/Fernwartung_Wolkenhof.exe", rmsFn);
            if (status != 0)
            {
                ReportAction($"Cannot install Remote Maintenance Tool. Error: {status}");
            }
            bWSoftware.ReportProgress(80);

            // Enable .NET Framework 3.5
            ReportAction("Installing Adobe Acrobat Reader DC ...");
            status = Core.Actions.AppxManagement.InstallFeature("NetFx3");
            switch (status)
            {
                case 0:
                    break;

                case 3010: // 3010 = ERROR_SUCCESS_REBOOT_REQUIRED
                    ReportAction($".NET Framework 3.5 was installed but a reboot is required!");
                    break;

                default:
                    ReportAction($".NET Framework 3.5 cannot be installed. Error: {status}");
                    break;
            }
            bWSoftware.ReportProgress(100);
            UnlockUI();
        }

        #endregion

        #region Functions

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

        private void ReportAction(string s)
        {
            Dispatcher.Invoke(new Action(() => {
                ActionLabel.Content = s;
            }), DispatcherPriority.ContextIdle);
        }

        #endregion

        #region Actions

        public void SwitchToAdmin_Click(object sender, RoutedEventArgs e) => bWSwitch.RunWorkerAsync();
        public void RemoveBloatware_Click(object sender, RoutedEventArgs e) => bWBloatware.RunWorkerAsync();
        public void InstallActionCard_Click(object sender, RoutedEventArgs e) => bWSoftware.RunWorkerAsync();
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) => ProgrBar.Value = e.ProgressPercentage;

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

        #endregion

    }
}
