using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KingsLib;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<data.GameAccount> gameAccounts = new List<data.GameAccount>();
        
        private void setDummyAccounts()
        {
            data.GameAccount oGA;

            oGA = new data.GameAccount();
            oGA.status = data.AccountStatus.Online;
            oGA.serverCode = "S35";
            oGA.nickName = "超級一六九";
            gameAccounts.Add(oGA);

            oGA = new data.GameAccount();
            oGA.status = data.AccountStatus.Offline;
            oGA.serverCode = "S37";
            oGA.nickName = "怕死的水子遠";
            gameAccounts.Add(oGA);
        }

        private void blindingAccounts()
        {
            setDummyAccounts();
            lvAccounts.ItemsSource = gameAccounts;
        }

    }
}
