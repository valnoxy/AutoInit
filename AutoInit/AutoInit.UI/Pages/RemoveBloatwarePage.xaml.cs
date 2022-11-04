using System;
using System.Collections.Generic;
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

namespace AutoInit.UI.Pages
{
    /// <summary>
    /// Interaktionslogik für RemoveBloatwarePage.xaml
    /// </summary>
    public partial class RemoveBloatwarePage : Page
    {
        public class App
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public bool IsChecked { get; set; }
        }

        private List<App> SysApps;
        public List<App> SysAppsList
        {
            get
            {
                return SysApps;
            }
        }

        private List<App> ThirdApps;
        public List<App> ThirdAppsList
        {
            get
            {
                return ThirdApps;
            }
        }


        public RemoveBloatwarePage()
        {
            InitializeComponent();

            SysApps = new List<App>();
            SysApps.Add(new App { Name = "3D Builder", ID = "Microsoft.3DBuilder", IsChecked = true});
            SysApps.Add(new App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer", IsChecked = true});
            SysApps.Add(new App { Name = "App Connector", ID = "Microsoft.Appconnector", IsChecked = true});
            SysApps.Add(new App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone", IsChecked = true});
            SysApps.Add(new App { Name = "Cortana", ID = "Microsoft.549981C3F5F10", IsChecked = true});
            SysApps.Add(new App { Name = "Get Help", ID = "Microsoft.GetHelp", IsChecked = true});
            SysApps.Add(new App { Name = "GroupMe", ID = "Microsoft.GroupMe10", IsChecked = true});
            SysApps.Add(new App { Name = "Messaging", ID = "Microsoft.Messaging", IsChecked = true});
            SysApps.Add(new App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal", IsChecked = true});
            SysApps.Add(new App { Name = "Mobile Plans", ID = "Microsoft.OneConnect", IsChecked = true});
            SysApps.Add(new App { Name = "MSN Finance", ID = "Microsoft.BingFinance", IsChecked = true});
            SysApps.Add(new App { Name = "MSN News", ID = "Microsoft.BingNews", IsChecked = true});
            SysApps.Add(new App { Name = "MSN Sports", ID = "Microsoft.BingSports", IsChecked = true});
            SysApps.Add(new App { Name = "MSN Weather", ID = "Microsoft.BingWeather", IsChecked = true});
            SysApps.Add(new App { Name = "People", ID = "Microsoft.People", IsChecked = true});
            SysApps.Add(new App { Name = "Print3D", ID = "Microsoft.Print3D", IsChecked = true});
            SysApps.Add(new App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes", IsChecked = true});
            SysApps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted", IsChecked = true});
            SysApps.Add(new App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder", IsChecked = true});
            SysApps.Add(new App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub", IsChecked = true});
            SysApps.Add(new App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms", IsChecked = true});
            SysApps.Add(new App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera", IsChecked = true});
            SysApps.Add(new App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox App", ID = "Microsoft.XboxApp", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider", IsChecked = true});
            SysApps.Add(new App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay", IsChecked = true});
            SysApps.Add(new App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone", IsChecked = true});
            SysApps.Add(new App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon", IsChecked = true});
            SysApps.Add(new App { Name = "Your Phone", ID = "Microsoft.YourPhone", IsChecked = true});


            ThirdApps = new List<App>();
            ThirdApps.Add(new App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga", IsChecked = true});
            ThirdApps.Add(new App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga", IsChecked = true});
            ThirdApps.Add(new App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491", IsChecked = true});
            ThirdApps.Add(new App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree", IsChecked = true});
            ThirdApps.Add(new App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager", IsChecked = true});
            ThirdApps.Add(new App { Name = "Flipboard", ID = "Flipboard.Flipboard", IsChecked = true});
            ThirdApps.Add(new App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio", IsChecked = true});
            ThirdApps.Add(new App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote", IsChecked = true});
            ThirdApps.Add(new App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection", IsChecked = true});
            ThirdApps.Add(new App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP", IsChecked = true});
            ThirdApps.Add(new App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub", IsChecked = true});
            ThirdApps.Add(new App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest", IsChecked = true});
            ThirdApps.Add(new App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2", IsChecked = true});
            ThirdApps.Add(new App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop", IsChecked = true});
            ThirdApps.Add(new App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop", IsChecked = true});
            ThirdApps.Add(new App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam", IsChecked = true});
            ThirdApps.Add(new App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic", IsChecked = true});
            ThirdApps.Add(new App { Name = "Sway", ID = "Microsoft.Office.Sway", IsChecked = true});
            ThirdApps.Add(new App { Name = "To Do app", ID = "Microsoft.Todos", IsChecked = true});
            ThirdApps.Add(new App { Name = "Twitter", ID = "9E2F88E3.Twitter", IsChecked = true});

            this.DataContext = this;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.IsChecked == true)
                Debug.WriteLine($"Checked {cb.Content}");
            else
                Debug.WriteLine($"Unchecked {cb.Content}");
        }

        private void SelectAllBtn(object sender, RoutedEventArgs e)
        {
            foreach (App app in SysApps)
            {
                Debug.WriteLine($"Checking {app.Name} ...");
                app.IsChecked = true;
            }
            foreach (App app in ThirdApps)
            {
                Debug.WriteLine($"Checking {app.Name} ...");
                app.IsChecked = true;
            }

            SysAppsItemList.BindingExpression.UpdateTarget();

        }

        private void DeSelectAllBtn(object sender, RoutedEventArgs e)
        {
            foreach (App app in SysApps)
            {
                app.IsChecked = false;
            }
            foreach (App app in ThirdApps)
            {
                app.IsChecked = false;
            }
        }
    }
}
