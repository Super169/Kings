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
        System.Timers.Timer autoTimer = new System.Timers.Timer(1000);

        private void goAutoKings()
        {
            normalMode = !normalMode;
            setUI();

            if (normalMode)
            {
                autoTimer.Enabled = false;
                UpdateResult("自動大皇帝 - 停止");
            }
            else
            {

                UpdateResult("自動大皇帝 - 啟動");
                autoTimer.Interval = 1000;
                autoTimer.Enabled = true;
            }

        }

        void autoTimerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            autoTimer.Enabled = false;
            UpdateResult(string.Format("自動大皇帝 - 開始執行"));

            DateTime minNext = DateTime.Now.AddMinutes(1);
            DateTime nextActionTime = new DateTime(minNext.Year, minNext.Month, minNext.Day, minNext.Hour, 00, 00).AddHours(1);
            // DateTime nextActionTime = new DateTime(minNext.Year, minNext.Month, minNext.Day, minNext.Hour, minNext.Minute, 00).AddMinutes(1);

            goAutoTasks();

            autoTimer.Interval = (int)(nextActionTime - DateTime.Now).TotalSeconds * 1000;
            autoTimer.Enabled = true;
            UpdateResult(string.Format("自動大皇帝 - 執行完成, 下次執行時候為: {0:yyyy-MM-dd hh:mm:ss}", nextActionTime));
        }

        private void goAutoTasks()
        {
            goCheckAccountStatus(true);
            goTaskHarvestAll();
            // goCycleShop();
            goTaskFinishAllTasks();
            goTaskSingInAll();
            goTaskReadEmail();
            goTaskSLBuyFood();
            goTaskCleanBag();
            goTasksIndustryBuyAll();
        }

        private void goCheckAccountStatus(bool forceCheck = false)
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                oGA.CheckStatus(forceCheck);
            }
           refreshAccountList();
        }

        private void goTaskHarvestAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline()) action.goManorHarvestAll(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskCycleShop()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline())
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
        }

        private void goTaskFinishAllTasks()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline()) action.goTaskFinishTaskAll(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskSingInAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline()) action.goSignIn(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskReadEmail()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline())
                {
                    int emailCnt = action.goEmailReadAll(oGA.currHeader, oGA.sid);
                    if (emailCnt > 0) UpdateResult(oGA.msgPrefix() + string.Format("開啟 {0} 封郵件", emailCnt));
                }
            }
        }

        private void goTaskSLBuyFood()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline())
                {
                    int buyCount = action.goSLShopBuyFood(oGA);
                    if (buyCount > 0) UpdateResult(oGA.msgPrefix() + string.Format("勢力商店買了 {0} 次糧", buyCount));
                }
            }
        }

        private void goTaskCleanBag()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline())
                {
                    int cleanUpCount = action.goBagCleanUp(oGA, UpdateInfoHandler);
                }
            }
        }

        private void goTasksIndustryBuyAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.IsOnline())
                {
                    // go buy food first, then silver
                    int buyCnt = action.goIndustryBuyAll(oGA, null, true, false);
                    
                    // can also buy food if possible
                    buyCnt += action.goIndustryBuyAll(oGA, null, true, true);

                    if (buyCnt > 0) UpdateResult(oGA.msgPrefix() + string.Format("在產業中購買了{0}次", buyCnt));
                }
            }

        }


    }
}
