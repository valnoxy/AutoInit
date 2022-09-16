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
            // add apps to list from RemApp() method
            apps.Add(new App { Name = "App Connector", ID = "Microsoft.Appconnector" });
            apps.Add(new App { Name = "Cortana", ID = "Microsoft.549981C3F5F10" });
            apps.Add(new App { Name = "Get Help", ID = "Microsoft.GetHelp" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });

            // appID = ID
            // Name = Removing x ...
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
            apps.Add(new App { Name = "Tips app", ID = "Microsoft.Getstarted" });
        }

        public static void RemApp()
        {
            int statuscode;
            Console.WriteLine("    -> Removing Messaging ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Messaging");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Mixed Reality Portal ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.MixedReality.Portal");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Windows Feedback Hub ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsFeedbackHub");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Windows Alarms ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsAlarms");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Windows Camera ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsCamera");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Windows Maps ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsMaps");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Minecraft for Windows 10 Edition ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.MinecraftUWP");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing People ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.People");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Print3D...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Print3D");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Mobile Plans ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.OneConnect");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Microsoft Solitaire Collection ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftSolitaireCollection");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Sticky Notes ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftStickyNotes");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing GroupMe ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.GroupMe10");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Voice Recorder ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsSoundRecorder");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing 3D Builder ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.3DBuilder");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing 3D Viewer ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Microsoft3DViewer");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing MSN Weather ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.BingWeather");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing MSN Sports ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.BingSports");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing MSN News ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.BingNews");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing MSN Finance ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.BingFinance");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing My Office ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.MicrosoftOfficeHub");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Office OneNote ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Office.OneNote");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Sway ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Office.Sway");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox App ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.XboxApp");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox Live in-game experience ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Xbox.TCUI");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox Game Bar ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.XboxGamingOverlay");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox Game Bar Plugin ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.XboxGameOverlay");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox Identity Provider ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.XboxIdentityProvider");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Xbox Speech to Text Overlay ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.XboxSpeechToTextOverlay");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Network Speedtest ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.NetworkSpeedTest");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing To Do app ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.Todos");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Shazam ...");
            
            statuscode = AppxRemove.RemoveAppx("ShazamEntertainmentLtd.Shazam");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Candy Crush ...");
            
            statuscode = AppxRemove.RemoveAppx("king.com.CandyCrushSaga");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            statuscode = AppxRemove.RemoveAppx("king.com.CandyCrushSodaSaga");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Flipboard ...");
            
            statuscode = AppxRemove.RemoveAppx("Flipboard.Flipboard");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Twitter ...");
            
            statuscode = AppxRemove.RemoveAppx("9E2F88E3.Twitter");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing iHeartRadio ...");
            
            statuscode = AppxRemove.RemoveAppx("ClearChannelRadioDigital.iHeartRadio");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Duolingo ...");
            
            statuscode = AppxRemove.RemoveAppx("D5EA27B7.Duolingo-LearnLanguagesforFree");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Photoshop Express ...");
            
            statuscode = AppxRemove.RemoveAppx("AdobeSystemIncorporated.AdobePhotoshop");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Pandora ...");
            
            statuscode = AppxRemove.RemoveAppx("PandoraMediaInc.29680B314EFC2");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Eclipse Manager ...");
            
            statuscode = AppxRemove.RemoveAppx("46928bounde.EclipseManager");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Code Writer ...");
            
            statuscode = AppxRemove.RemoveAppx("ActiproSoftwareLLC.562882FEEB491");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Spotify ...");
            
            statuscode = AppxRemove.RemoveAppx("SpotifyAB.SpotifyMusic");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Your Phone Companion ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.WindowsPhone");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            statuscode = AppxRemove.RemoveAppx("Microsoft.Windows.Phone");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Communications - Phone app ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.CommsPhone");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Your Phone ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.YourPhone");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
            Console.WriteLine("    -> Removing Remote Desktop app ...");
            
            statuscode = AppxRemove.RemoveAppx("Microsoft.RemoteDesktop");
            if (statuscode != 0)
                Console.WriteLine("[!] App cannot be removed!");
            // ---------------------------------------------------------------------------
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
