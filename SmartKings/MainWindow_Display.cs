using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private string SystemTimePrefix()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ");
        }

        private void UpdateTextBox(TextBox tb, string info, bool addTime = true, bool resetText = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateTextBox(tb, info, false, resetText)));
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += SystemTimePrefix();
            tb.Text += info + "\n";
            tb.ScrollToEnd();
        }


        private void UpdateStatus(string info, bool addTime = true, bool resetText = false)
        {
            UpdateTextBox(txtStatus, info, addTime, resetText);
        }

    }
}
