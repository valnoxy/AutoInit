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
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void SwitchActionCard_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SwitchToAdminPage());
        }

        private void RemoveActionCard_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RemoveBloatwarePage());
        }
    }
}
