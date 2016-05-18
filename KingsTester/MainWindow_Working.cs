using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KingsTester
{
    public partial class MainWindow : Window
    {
        private void kingsWorkingTester()
        {
            go_TestTravel();
        }

        private void go_TestNavalWar()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            bool sendOK = action.sendNavalTroops(oGA, 3, oGA.BossWarBody);
            UpdateResult(oGA.msgPrefix() + (sendOK ? "跨服入侵準備完成" : "跨服入侵準備失敗"));
        }

        private void go_TestTravel()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;

            RequestReturnObject rro;
            rro = action.go_Travel_getMapInfo(oGA.currHeader, oGA.sid);
            UpdateResult("\n" + rro.responseText);

        }
    }
}
