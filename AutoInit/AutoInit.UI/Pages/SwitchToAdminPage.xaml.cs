using System;
using System.Collections.Generic;
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

namespace AutoInit.UI.Pages
{
    /// <summary>
    /// Interaktionslogik für SwitchToAdminPage.xaml
    /// </summary>
    public partial class SwitchToAdminPage : Page
    {
        public SwitchToAdminPage()
        {
            InitializeComponent();

            CurrentUserLb.Content = $"Current User: {Environment.UserName}";

            Config.SwitchToAdmin.Password = PasswordBox.Password;
            Config.SwitchToAdmin.RemoveCurrentUser = RemoveUserCb.IsChecked.Value;
        }

        private void PasswortBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.SwitchToAdmin.Password = PasswordBox.Password;
        }

        private void RemoveUserCb_Checked(object sender, RoutedEventArgs e)
        {
            Config.SwitchToAdmin.RemoveCurrentUser = RemoveUserCb.IsChecked.Value;
        }
    }
}
