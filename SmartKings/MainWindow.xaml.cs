using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KingsLib.monitor;
using static KingsLib.monitor.KingsMonitor;
using KingsLib;

namespace SmartKings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = ((App)Application.Current).winTitle;
            UpdateStatus(this.Title + " 啟動");
            com.start("SmartKings");
            blindingAccounts();

            KingsMonitor.notificationEventHandler += new NotificationEventHandler(this.OnNotificationHandler);
            KingsMonitor.newSidEventHandler += new NewSidEventHandler(this.OnNewSidHandler);

            if (!KingsMonitor.Start())
            {
                MessageBox.Show("啟動監察器失敗");
                this.Close();
            }

        }

    }
}
