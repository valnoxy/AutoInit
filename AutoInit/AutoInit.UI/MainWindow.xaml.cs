using AutoInit.Core;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchToAdmin_Click(object sender, RoutedEventArgs e)
        {
            ActionLabel.Content = "Switching to Administrator account ...";
            ProgBar.Value = 0;
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
            ProgBar.Value = 33;

            ActionLabel.Content = "Change Administrator password ...";
            p.StartInfo.Arguments = $"/c \"net user Administrator {TbPassword.Text}\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                ActionLabel.Content = $"Cannot change password from Administrator account! Error: {p.ExitCode}";
                ProgBar.Value = 0;
                return;
            }
            ProgBar.Value = 66;

            //if (Configuration.RemoveDefaultUser)
            //{
            ActionLabel.Content = "Remove account 'User' ...";
            p.StartInfo.Arguments = $"/c \"net user User /delete\"";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                ActionLabel.Content = $"Cannot delete account 'User'! Error: {p.ExitCode}";
                ProgBar.Value = 0;
                return;
            }
            //}
            ProgBar.Value = 100;
            ActionLabel.Content = "Successfully switched to Administrator. Please log out now.";
        }

        private void RemoveBloatware_Click(object sender, RoutedEventArgs e)
        {
            int statuscode;
            ActionLabel.Content = "Removing bloatware ...";
            ProgBar.Value = 0;

            // Initialize list of apps to remove
            AppxRemove.InitApps();

            // foreach app in list, remove app with appxremove and add to progress bar
            foreach (var app in AppxRemove.apps)
            {
                statuscode = AppxRemove.RemoveAppx(app.ID);
                if (statuscode != 0)
                {
                    ActionLabel.Content = $"Cannot remove {app.Name}! Error: {statuscode}";
                    ProgBar.Value = 0;
                    return;
                }
                ProgBar.Value += 100 / AppxRemove.apps.Count;
            }

            Console.WriteLine("[i] Bloatware removed!");
        }
    }
}
