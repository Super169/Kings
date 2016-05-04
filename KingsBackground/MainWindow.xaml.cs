using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KingsBackground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        System.Timers.Timer myTimer = new System.Timers.Timer(1000);

        public MainWindow()
        {
            InitializeComponent();
            myTimer.Interval = 1000;
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateCount);
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            btnGo.IsEnabled = false;
            ((App)Application.Current).SetNotifyText("Timer running");
            myTimer.Enabled = true;
        }

         private void UpdateCount(object sender, ElapsedEventArgs e)
        {
            UpdateUI((++count).ToString()); 
        }

        private void UpdateUI(string info)
        {
            Dispatcher currDispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            if (currDispatcher == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateUI(info)));
                return;
            }

            lblCount.Content = info;
        }

    }
}
