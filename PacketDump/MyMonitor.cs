using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using PacketData;

namespace PacketDump
{
    static class MyMonitor
    {

        static List<MySocketMonitor> monitorList = new List<MySocketMonitor>();

        static Object accountsLocker = new Object();

        public delegate void NotificationEventHandler(string info);
        public static event NotificationEventHandler notificationEventHandler;

        public delegate void NewPacketEventHandler(MyPacket p);
        public static event NewPacketEventHandler newPacketEventHandler;

        public static bool Start()
        {
            monitorList.Clear();
            IPAddress[] hosts = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (hosts == null || hosts.Length == 0)
            {

                return false;
            }
            for (int i = 0; i < hosts.Length; i++)
            {
                MySocketMonitor monitor = new MySocketMonitor(hosts[i]);
                monitor.newPacketEventHandler += new MySocketMonitor.NewPacketEventHandler(onNewPacketHandler);
                monitorList.Add(monitor);
            }

            int monitorCnt = 0;

            foreach (MySocketMonitor monitor in monitorList)
            {
                if (monitor.Start())
                {
                    UpdateUI("開始監察 " + monitor.ip);
                    monitorCnt++;
                }
            }

            return (monitorCnt > 0);
        }


        public static bool Stop()
        {
            foreach (MySocketMonitor monitor in monitorList)
            {
                if (monitor.IsReady())
                {
                    monitor.Stop();
                    UpdateUI("停止監察 " + monitor.ip);
                }
            }
            return true;
        }

        private static void onNewPacketHandler(MySocketMonitor monitor, MyPacket p)
        {
            if (newPacketEventHandler != null)
            {
                newPacketEventHandler(p);
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
