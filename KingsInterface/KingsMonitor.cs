using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingsInterface.monitor;
using KingsInterface.data;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using Fiddler;

namespace KingsInterface
{
    public static class KingsMonitor
    {
        static List<KingsSocketMonitor> monitorList = new List<KingsSocketMonitor>();

        static List<PacketKey> accounts = new List<PacketKey>();
        static Object accountsLocker = new Object();

        // Propagate to parent handler when new sid is detected  
        public delegate void NewSidEventHandler(LoginInfo li, HTTPRequestHeaders oH);
        public static event NewSidEventHandler newSidEventHandler;

        public delegate void NotificationEventHandler(string info);
        public static event NotificationEventHandler notificationEventHandler;

        public static bool Start(string appName)
        {
            monitorList.Clear();
            IPAddress[] hosts = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (hosts == null || hosts.Length == 0)
            {

                #if CONSOLE_DEBUG
                    Console.WriteLine("No hosts detected, please check your network!");
                #endif
                return false;
            }
            for (int i = 0; i < hosts.Length; i++)
            {
                KingsSocketMonitor monitor = new KingsSocketMonitor(hosts[i]);
                monitor.newPacketEventHandler += new KingsSocketMonitor.NewPacketEventHandler(onNewPacketHandler);
                monitorList.Add(monitor);
                // UpdateUI("Monitor added on " + hosts[i].ToString());
            }

            // Start Fiddler before starting the monitor, otherwise, it may cause problem when traffic comes before not all monitors started.
            com.ConfigFiddler(appName);
            com.Startup(false);

            int monitorCnt = 0;

            foreach (KingsSocketMonitor monitor in monitorList)
            {
#if CONSOLE_DEBUG
                Console.WriteLine("Start monitor on " + monitor.ip());
#endif
                if (monitor.Start()) monitorCnt++;
            }

            return (monitorCnt > 0);
        }


        public static bool Stop()
        {
            foreach(KingsSocketMonitor monitor in monitorList)
            {
#if CONSOLE_DEBUG
                Console.WriteLine("Stop monitor on " + monitor.ip());
#endif
                monitor.Stop();
            }
            return true;
        }

        private static void onNewPacketHandler(KingsSocketMonitor monitor, KingsPacket p)
        {
            Thread T = new Thread(() => CheckNewPacket(p));
            T.Start();
        }

        private static void CheckNewPacket(KingsPacket p)
        {
            string server = "";
            string sid = "";

            string rx = "(kings[0-9]+)\\.icantw.com.+\"sid\":\"([a-z0-9]+)\"";
            Match match = Regex.Match(p.data, rx);
            if (!match.Success) return;
            server = match.Groups[1].Value;
            sid = match.Groups[2].Value;
            UpdateUI(string.Format("Server: {0} ; sid: {1}", server, sid));
            
            PacketKey oAK = null;
            lock (accountsLocker)
            {
                if (!accounts.Exists(x => x.sid == sid))
                {
                    oAK = new PacketKey() { sid = sid };
                    accounts.Add(oAK);
                }
            }
            if (oAK == null) return;

            UpdateUI("*** Find new sid: " + sid);

            HTTPRequestHeaders oH = new HTTPRequestHeaders();
            oH.HTTPMethod = "POST";
            oH.HTTPVersion = "HTTP/1.1";
            byte[] rawPath = { 47, 109, 46, 100, 111 };
            oH.RawPath = rawPath;
            oH.RequestPath = "/m.do";
            oH.UriScheme = "http";

            string[] headerStr = p.data.Split('|');
            foreach (string s in headerStr)
            {
                if ((s.Trim() != "") && !s.StartsWith("POST ") && !s.StartsWith("{") && s.Contains(":"))
                {
                    string[] pair = s.Split(':');
                    string key = pair[0].Trim();
                    string value = pair[1].Trim();
                    if ((key != "") && (value != ""))
                    {
                        oH[key] = value;
                        // UpdateUI(string.Format("{0} : {1}", key, value));
                    }
                }
            }
            
            data.LoginInfo li = action.goGetAccountInfo(oH, sid);
            if (!li.ready) return;

            li.server = server;
            UpdateUI(string.Format("{0} | {1} - {2}", li.account, li.serverTitle, li.nickName));
            NewSid(li, oH);

        }


        private static void NewSid(LoginInfo li, HTTPRequestHeaders oH)
        {
            if (newSidEventHandler != null)
            {
                newSidEventHandler(li, oH);
            }
        }

        private static void UpdateUI(string info)
        {
            if (notificationEventHandler != null)
            {
                notificationEventHandler(info);
            }
        }


    }
}
