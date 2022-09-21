using AutoInit.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace AutoInit.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private BackgroundWorker applyBackgroundWorker;
        bool IsCanceled = false;
        
        public MainWindow()
        {
            InitializeComponent();
            applyBackgroundWorker = new BackgroundWorker();
            applyBackgroundWorker.WorkerReportsProgress = true;
            applyBackgroundWorker.WorkerSupportsCancellation = true;
            applyBackgroundWorker.ProgressChanged += applyBackgroundWorker_ProgressChanged;
        }

        private void applyBackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            //
            // Value list
            // --------------------------
            // Progressbar handling
            //   101: Progressbar is not Indeterminate
            //   102: Progressbar is Indeterminate
            //
            // Standard Message handling
            //   201: ProgText -> Prepare Disk
            //   202: ProgText -> Apply WIM
            //   203: ProgText -> Install Bootloader
            //   204: ProgText -> Install recovery
            //   205: ProgText -> Install unattend.xml
            //   250: Installation complete
            //
            // Error message handling
            //   301: Failed at preparing disk
            //   302: Failed at applying WIM
            //   303: Failed at installing bootloader
            //   304: Failed at installing recovery
            //   305: Failed at installing unattend.xml
            //
            // Range 0-100 -> Progressbar percentage
            //

            // Progress bar handling
            switch (e.ProgressPercentage)
            {
                #region Progress bar settings
                case 101:                           // 101: Progressbar is not Indeterminate
                    ProgrBar.IsIndeterminate = false;
                    break;
                case 102:                           // 102: Progressbar is Indeterminate
                    ProgrBar.IsIndeterminate = true;
                    break;
                #endregion

                #region Standard message handling
                case 201:                           // 201: ProgText -> Prepare Disk
                    ActionLabel.Content = "Preparing disk ...";
                    break;
                case 202:                           // 202: ProgText -> Applying WIM
                    ActionLabel.Content = $"Applying Image to disk ({ProgrBar.Value}%) ...";
                    break;
                case 203:                           // 203: ProgText -> Installing Bootloader
                    ActionLabel.Content = "Installing bootloader to disk ...";
                    break;
                case 204:                           // 204: ProgText -> Installing recovery
                    ActionLabel.Content = "Registering recovery partition to Windows ...";
                    break;
                case 205:                           // 205: ProgText -> Installing unattend.xml
                    ActionLabel.Content = "Copying unattend.xml to disk ...";
                    break;
                case 250:                           // 250: Installation complete
                    ActionLabel.Content = "Installation completed. Press 'Next' to restart your computer.";
                    ProgrBar.Value = 100;
                    break;
                #endregion

                #region Error message handling
                case 301:                           // 301: Failed to delete User
                    ActionLabel.Content = "Cannot delete account 'User'!";
                    ProgrBar.Value = 0;
try {}
                    IsCanceled = true;
                    break;
                case 302:                           // 302: Failed at applying WIM
                    ActionLabel.Content = "Failed at applying WIM. Please check your image and try again.";
                    ProgrBar.Value = 0;
                    if (ApplyContent.ContentWindow != null)
                    {
                        ApplyContent.ContentWindow.NextBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.BackBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    if (CloudContent.ContentWindow != null)
                    {
                        CloudContent.ContentWindow.NextBtn.IsEnabled = false;
                        CloudContent.ContentWindow.BackBtn.IsEnabled = false;
                        CloudContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    IsCanceled = true;
                    break;
                case 303:                           // 303: Failed at installing bootloader
                    ActionLabel.Content = "Failed at installing bootloader. Please check your image and try again.";
                    ProgrBar.Value = 0;
                    if (ApplyContent.ContentWindow != null)
                    {
                        ApplyContent.ContentWindow.NextBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.BackBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    if (CloudContent.ContentWindow != null)
                    {
                        CloudContent.ContentWindow.NextBtn.IsEnabled = false;
                        CloudContent.ContentWindow.BackBtn.IsEnabled = false;
                        CloudContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    IsCanceled = true;
                    break;
                case 304:                           // 304: Failed at installing recovery
                    ActionLabel.Content = "Failed at installing recovery. Please check your image and try again.";
                    ProgrBar.Value = 0;
                    if (ApplyContent.ContentWindow != null)
                    {
                        ApplyContent.ContentWindow.NextBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.BackBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    if (CloudContent.ContentWindow != null)
                    {
                        CloudContent.ContentWindow.NextBtn.IsEnabled = false;
                        CloudContent.ContentWindow.BackBtn.IsEnabled = false;
                        CloudContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    IsCanceled = true;
                    break;
                case 305:                           // 305: Failed at installing unattend.xml
                    ActionLabel.Content = "Failed at copying unattend.xml to disk. Please check your image or config and try again.";
                    ProgrBar.Value = 0;
                    if (ApplyContent.ContentWindow != null)
                    {
                        ApplyContent.ContentWindow.NextBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.BackBtn.IsEnabled = false;
                        ApplyContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    if (CloudContent.ContentWindow != null)
                    {
                        CloudContent.ContentWindow.NextBtn.IsEnabled = false;
                        CloudContent.ContentWindow.BackBtn.IsEnabled = false;
                        CloudContent.ContentWindow.CancelBtn.IsEnabled = true;
                    }
                    IsCanceled = true;
                    break;
                    #endregion
            }

            // Progressbar percentage
            if (e.ProgressPercentage <= 100)
                this.ProgrBar.Value = e.ProgressPercentage;
        }

        public void SwitchToAdmin(object? sender, DoWorkEventArgs e)
        {
            ActionLabel.Content = "Switching to Administrator account ...";
            ProgrBar.Value = 0;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c \"net user Administrator /active:yes\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                ActionLabel.Content = $"Cannot enable Administrator account! Error: {p.ExitCode}";
                return;
            }
            ProgrBar.Value = 33;

            ActionLabel.Content = "Change Administrator password ...";
            p.StartInfo.Arguments = $"/c \"net user Administrator {TbPassword.Text}\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                ActionLabel.Content = $"Cannot change password from Administrator account! Error: {p.ExitCode}";
                ProgrBar.Value = 0;
                return;
            }
            ProgrBar.Value = 66;

            //if (Configuration.RemoveDefaultUser)
            //{
            ActionLabel.Content = "Remove account 'User' ...";
            p.StartInfo.Arguments = $"/c \"net user User /delete\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                ActionLabel.Content = $"Cannot delete account 'User'! Error: {p.ExitCode}";
                ProgrBar.Value = 0;
                return;
            }
            //}
            ProgrBar.Value = 100;
            ActionLabel.Content = "Successfully switched to Administrator. Please log out now.";
        }
        
        private void SwitchToAdmin_Click(object sender, RoutedEventArgs e)
        {
            applyBackgroundWorker.DoWork += SwitchToAdmin;
            applyBackgroundWorker.RunWorkerAsync();
        }

        private void RemoveBloatware_Click(object sender, RoutedEventArgs e)
        {
            int statuscode;
            ActionLabel.Content = "Removing bloatware ...";
            ProgrBar.Value = 0;

            // Initialize list of apps to remove
            AppxRemove.InitApps();

            // foreach app in list, remove app with appxremove and add to progress bar
            foreach (var app in AppxRemove.apps)
            {
                statuscode = AppxRemove.RemoveAppx(app.ID);
                if (statuscode != 0)
                {
                    ActionLabel.Content = $"Cannot remove {app.Name}! Error: {statuscode}";
                    ProgrBar.Value = 0;
                    return;
                }
                ProgrBar.Value += 100 / AppxRemove.apps.Count;
            }

            Console.WriteLine("[i] Bloatware removed!");
        }
    }
}
