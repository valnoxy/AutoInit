using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;


namespace AutoInit.Core
{
    public class AppxRemove
    {
        // public list of apps to remove
        public class App
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
        public static List<App> apps;

        public static void InitApps()
        {
            apps.Add(new App { Name = "App Connector", ID = "Microsoft.Appconnector" });
            apps.Add(new App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
            apps.Add(new App { Name = "Get Help", ID = "Microsoft.GetHelp" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Messaging", ID = "Microsoft.Messaging" });
            apps.Add(new App { Name = "Mixed Reality Portal", ID = "Microsoft.MixedReality.Portal" });
            apps.Add(new App { Name = "Windows Feedback Hub", ID = "Microsoft.WindowsFeedbackHub" });
            apps.Add(new App { Name = "Windows Alarms", ID = "Microsoft.WindowsAlarms" });
            apps.Add(new App { Name = "Windows Camera", ID = "Microsoft.WindowsCamera" });
            apps.Add(new App { Name = "Windows Maps", ID = "Microsoft.WindowsMaps" });
            apps.Add(new App { Name = "Minecraft for Windows 10 Edition", ID = "Microsoft.MinecraftUWP" });
            apps.Add(new App { Name = "People", ID = "Microsoft.People" });
            apps.Add(new App { Name = "Print3D", ID = "Microsoft.Print3D" });
            apps.Add(new App { Name = "Mobile Plans", ID = "Microsoft.OneConnect" });
            apps.Add(new App { Name = "Microsoft Solitaire Collection", ID = "Microsoft.MicrosoftSolitaireCollection" });
            apps.Add(new App { Name = "Sticky Notes", ID = "Microsoft.MicrosoftStickyNotes" });
            apps.Add(new App { Name = "GroupMe", ID = "Microsoft.GroupMe10" });
            apps.Add(new App { Name = "Voice Recorder", ID = "Microsoft.WindowsSoundRecorder" });
            apps.Add(new App { Name = "3D Builder", ID = "Microsoft.3DBuilder" });
            apps.Add(new App { Name = "3D Viewer", ID = "Microsoft.Microsoft3DViewer" });
            apps.Add(new App { Name = "MSN Weather", ID = "Microsoft.BingWeather" });
            apps.Add(new App { Name = "MSN Sports", ID = "Microsoft.BingSports" });
            apps.Add(new App { Name = "MSN News", ID = "Microsoft.BingNews" });
            apps.Add(new App { Name = "MSN Finance", ID = "Microsoft.BingFinance" });
            apps.Add(new App { Name = "My Office", ID = "Microsoft.MicrosoftOfficeHub" });
            apps.Add(new App { Name = "Microsoft Office OneNote", ID = "Microsoft.Office.OneNote" });
            apps.Add(new App { Name = "Sway", ID = "Microsoft.Office.Sway" });
            apps.Add(new App { Name = "Xbox App", ID = "Microsoft.XboxApp" });
            apps.Add(new App { Name = "Xbox Live in-game experience", ID = "Microsoft.Xbox.TCUI" });
            apps.Add(new App { Name = "Xbox Game Bar", ID = "Microsoft.XboxGamingOverlay" });
            apps.Add(new App { Name = "Xbox Game Bar Plugin", ID = "Microsoft.XboxGameOverlay" });
            apps.Add(new App { Name = "Xbox Identity Provider", ID = "Microsoft.XboxIdentityProvider" });
            apps.Add(new App { Name = "Xbox Speech to Text Overlay", ID = "Microsoft.XboxSpeechToTextOverlay" });
            apps.Add(new App { Name = "Network Speedtest", ID = "Microsoft.NetworkSpeedTest" });
            apps.Add(new App { Name = "To Do app", ID = "Microsoft.Todos" });
            apps.Add(new App { Name = "Shazam", ID = "ShazamEntertainmentLtd.Shazam" });
            apps.Add(new App { Name = "Candy Crush Saga", ID = "king.com.CandyCrushSaga" });
            apps.Add(new App { Name = "Candy Crush Soda Saga", ID = "king.com.CandyCrushSodaSaga" });
            apps.Add(new App { Name = "Flipboard", ID = "Flipboard.Flipboard" });
            apps.Add(new App { Name = "Twitter", ID = "9E2F88E3.Twitter" });
            apps.Add(new App { Name = "iHeartRadio", ID = "ClearChannelRadioDigital.iHeartRadio" });
            apps.Add(new App { Name = "Duolingo", ID = "D5EA27B7.Duolingo-LearnLanguagesforFree" });
            apps.Add(new App { Name = "Photoshop Express", ID = "AdobeSystemIncorporated.AdobePhotoshop" });
            apps.Add(new App { Name = "Pandora", ID = "PandoraMediaInc.29680B314EFC2" });
            apps.Add(new App { Name = "Eclipse Manager", ID = "46928bounde.EclipseManager" });
            apps.Add(new App { Name = "Code Writer", ID = "ActiproSoftwareLLC.562882FEEB491" });
            apps.Add(new App { Name = "Spotify", ID = "SpotifyAB.SpotifyMusic" });
            apps.Add(new App { Name = "Your Phone Companion #1", ID = "Microsoft.WindowsPhone" });
            apps.Add(new App { Name = "Your Phone Companion #2", ID = "Microsoft.Windows.Phon" });
            apps.Add(new App { Name = "Communications - Phone app", ID = "Microsoft.CommsPhone" });
            apps.Add(new App { Name = "Your Phone", ID = "Microsoft.YourPhone" });
            apps.Add(new App { Name = "Remote Desktop app", ID = "Microsoft.RemoteDesktop" });
        }

        public static int RemoveAppx(string appID)
        {
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.CreateNoWindow = false;
            psi.Arguments = $"Get-AppxPackage '{appID}' | Remove-AppxPackage";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = "powershell.exe";
            var proc = Process.Start(psi);

            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}
