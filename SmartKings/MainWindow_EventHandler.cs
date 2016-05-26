using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        public void WindowPreClose()
        {
            saveAccounts();
        }

        private void OnNotificationHandler(string info)
        {
            // UpdateInfo(info);
        }

        private void OnNewSidHandler(LoginInfo li, HTTPRequestHeaders oH)
        {

            // UpdateStatus(string.Format("Server: {0}, SID: {1}, {2} - {3}", li.server, li.sid, li.serverTitle, li.nickName));
            UpdateAccountList(li, oH);
        }
    }
}
