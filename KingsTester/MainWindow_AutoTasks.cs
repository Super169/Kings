using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace KingsTester
{
    public partial class MainWindow : Window
    {

        private void goAutoTasks()
        {
            goHarvestAll();
            goCycleShop();
            goFinishAllTasks();
            goSingInAll();
        }

        private void goHarvestAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                action.goManorHarvestAll(oGA, UpdateInfoHandler);
            }
        }

        private void goCycleShop()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                List<CycleShopInfo> csis = action.goShopGetCycleShopInfo(oGA.currHeader, oGA.sid);
                foreach (CycleShopInfo csi in csis)
                {
                    // No way, can only check using this string, or hard code the position.  Text has been converted to TradChinese
                    if ((csi.pos < 3) && (!csi.sold) && (csi.res == "銀兩"))
                    {
                        string info = oGA.msgPrefix() + "用銀買 " + csi.nm + " : ";
                        if (action.goShopbuyCycleShopItem(oGA.currHeader, oGA.sid, csi.pos))
                        {
                            info += "成功";
                        }
                        else
                        {
                            info += "失敗";
                        }
                        UpdateResult(info);
                    }
                }
            }
        }

        private void goFinishAllTasks()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                action.goTaskFinishTaskAll(oGA, UpdateInfoHandler);
            }
        }

        private void goSingInAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                action.goSignIn(oGA.currHeader, oGA.sid, UpdateInfoHandler);
            }
        }

    }
}
