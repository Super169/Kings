using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class com
    {

        private const int FIDDLER_PORT = 8899;
        public static bool isSysProxy { get; private set; } = false;

        public static void start(string appName)
        {
            // Start Fiddler before starting the monitor, otherwise, it may cause problem when traffic comes before not all monitors started.
            ConfigFiddler(appName);
            Startup(false);
        }

        public static void ConfigFiddler(string appName)
        {
            Fiddler.FiddlerApplication.SetAppDisplayName(appName);

            Fiddler.FiddlerApplication.OnNotification += delegate (object sender, NotificationEventArgs oNEA)
            {
            };

            Fiddler.FiddlerApplication.Log.OnLogString += delegate (object sender, LogEventArgs oLEA)
            {
            };

            Fiddler.FiddlerApplication.AfterSessionComplete += delegate (Fiddler.Session oS)
            {
            };

            Fiddler.CONFIG.IgnoreServerCertErrors = false;

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);
        }

        //  System proxy is not recommended, just provide this option for future use if needed
        public static void Startup(bool sysProxy = false)
        {
            if (!FiddlerApplication.IsStarted())
            {
                FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;
                if (!sysProxy) oFCSF &= ~FiddlerCoreStartupFlags.RegisterAsSystemProxy;
                isSysProxy = sysProxy;
                Fiddler.FiddlerApplication.Startup(FIDDLER_PORT, oFCSF);
                Thread.Sleep(500);
            }
        }

        public static bool isStarted()
        {
            return FiddlerApplication.IsStarted();
        }

        public static void ShutDown()
        {
            if (FiddlerApplication.IsStarted()) FiddlerApplication.Shutdown();
            Thread.Sleep(500);
        }


    }
}
