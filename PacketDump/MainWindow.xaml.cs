using PacketData;
using PacketDump;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using static PacketDump.MyMonitor;

namespace PacketDump
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MyPacket> packets = new List<MyPacket>();
        private Object packetsLocker = new object();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "PacketDump  v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void UpdateInfo(string info, bool addTime = true, bool resetText = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateInfo(info, false, resetText)));
                return;
            }
            if (resetText) txtInfo.Text = "";
            if (addTime) txtInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ");
            txtInfo.Text += info + "\n";
            txtInfo.ScrollToEnd();
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnEnd.IsEnabled = true;
            btnClose.IsEnabled = false;
            MyMonitor.notificationEventHandler += new NotificationEventHandler(this.onNotificationHandler);
            MyMonitor.newPacketEventHandler += new NewPacketEventHandler(this.onNewPacketHandler);
            MyMonitor.Start();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            btnEnd.IsEnabled = false;
            MyMonitor.Stop();
            // wait 1 seconds for all socket event completed
            Thread.Sleep(1000);
            string path = Directory.GetCurrentDirectory();
            string fileName = path + "\\MyPackets.dat";
            FileStream fs = new FileStream("MyPackets.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, packets);
                UpdateInfo("數據包成功儲存到 " + fileName);
            }
            catch (Exception ex)
            {
                UpdateInfo("儲存數據包出錯\n" + ex.Message);
            } finally
            {
                fs.Close();
            }
            btnClose.IsEnabled = true;
        }

        private void onNotificationHandler(string info)
        {
            UpdateInfo(info);
        }

        private void onNewPacketHandler(MyPacket p)
        {
            lock (packetsLocker)
            {
                packets.Add(p);
            }
            if (packets.Count % 10 == 0)
            {
                UpdateInfo(packets.Count.ToString() + " packets received");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }
    }

}
