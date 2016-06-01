using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace KingsTester
{
    public partial class MainWindow : Window
    {
        System.Timers.Timer autoTimer = new System.Timers.Timer(1000);

        private bool bossWarDay()
        {
            int dow = auto.ScheduleInfo.getGameDOW();
            if ((dow != 0) && (dow != 5)) return false;
            return true;
        }

        private void goAutoTasks()
        {
            // 檢查帳戶
            goCheckAccountStatus(true);
            // 封地收獲
            goTaskHarvestAll();
            // 東瀛寶船
            goTaskCycleShop();
            // 每日簽到  --20160601, signin changed
            // goTaskSingInAll();
            // 為 "糧草先行" 任務購物
            goTaskMarketTask();
            // 勢力商店購買糧食
            goTaskSLBuyFood();
            // 產業購買
            goTasksIndustryBuyAll();
            // 佈置跨服入侵
            goTaskNavalWar();
            // 幸運轉盤
            goTaskLuckyCycle();
            // 領取團購寶箱
            goTaskTuanGou();
            // 拆紅包
            goTaskShuangShiyiActivityReward();
            // 購買討伐次數
            goTaskEliteBuyTime();
            // 購買英雄試練次數
            goTaskTrialsBuyTimes();
            // 領取 天下比武 及 三軍演武 獎勵
            goTaskGetArenaReward();
            // 周遊天下
            goTaskTravel();
            // 讀取郵件並取得附件
            goTaskReadEmail();
            // 取得已完成任務的獎勵
            goTaskFinishAllTasks();
            // 清理背包
            goTaskCleanBag();
        }


        void reloadAll()
        {
            // Restart Chrom at date start; may also add at 12:00 in case server down
            Process.Start("chrome.exe", "http://www.pubgame.tw/play.do?gc=king&gsc=35");
            Thread.Sleep(60000);
            Process.Start("chrome.exe", "http://www.pubgame.tw/play.do?gc=king&gsc=36");
            Thread.Sleep(60000);
            Process.Start("chrome.exe", "http://www.pubgame.tw/play.do?gc=king&gsc=37");
            Thread.Sleep(60000);
            Process.Start("chrome.exe", "http://www.pubgame.tw/play.do?gc=king&gsc=43");
            Thread.Sleep(60000);
        }

        void autoTimerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            autoTimer.Enabled = false;

            UpdateTextBox(txtLastExecution, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), false);
            UpdateTextBox(txtNextExecution, "執行中。。。。。。", false);

            UpdateResult(string.Format("自動大皇帝 - 開始執行"));
            

            DateTime minNext;
            DateTime nextActionTime;
            // DateTime nextActionTime = new DateTime(minNext.Year, minNext.Month, minNext.Day, minNext.Hour, minNext.Minute, 00).AddMinutes(1);

            DateTime now = DateTime.Now;
            int dow = auto.ScheduleInfo.getGameDOW();
            bool goBossWar = false;
            bool waitForBossWar = false;
            bool skipAction = false;

            TimeSpan bossWarStart = new TimeSpan(19, 59, 0);
            TimeSpan bossWarEnd = new TimeSpan(20, 31, 0);

            // Auto-reload at 6:05 & 9:05
            if ((now.Hour == 6) || (now.Hour == 9))
            {
                reloadAll();
            }

            if ((dow == 0) || (dow == 5))
            {
                TimeSpan currTS = new TimeSpan(now.Hour, now.Minute, now.Second);
                if (currTS < bossWarEnd)
                {
                    if (currTS > bossWarStart)
                    {
                        goBossWar = true;
                    }
                    else
                    {
                        TimeSpan waitTime = (bossWarStart - currTS);
                        int waitMin = (int)waitTime.TotalMinutes;
                        if (waitMin < 70)
                        {
                            waitForBossWar = true;
                            if (waitMin < 10) skipAction = true;
                        }
                    }
                }
            }

            if (goBossWar)
            {
                goTaskBossWar();
                nextActionTime = now.AddSeconds(31);

            }
            else if (skipAction)
            {
                UpdateResult(string.Format("是次行動取消, 直接等待神將無雙."));
                // For safety, add 1 more seconds here.
                nextActionTime = new DateTime(now.Year, now.Month, now.Day).Add(bossWarStart).AddSeconds(1);
            }
            else
            {
                goAutoTasks();
                if (waitForBossWar)
                {
                    UpdateResult(string.Format("下次行動設定為 神將無雙 開始時間."));
                    // For safety, add 1 more seconds here.
                    nextActionTime = new DateTime(now.Year, now.Month, now.Day).Add(bossWarStart).AddSeconds(1);
                }
                else
                {
                    minNext = DateTime.Now.AddMinutes(1);
                    nextActionTime = new DateTime(minNext.Year, minNext.Month, minNext.Day, minNext.Hour, 05, 00).AddHours(1);
                }

            }

            double waitMS = (nextActionTime - DateTime.Now).TotalSeconds * 1000;
            if (waitMS < 0) waitMS = 1000;

            autoTimer.Interval = waitMS;
            autoTimer.Enabled = true;
            UpdateResult(string.Format("自動大皇帝 - 執行完成, 下次執行時候為: {0:yyyy-MM-dd HH:mm:ss}", nextActionTime));
            UpdateTextBox(txtNextExecution, nextActionTime.ToString("yyyy-MM-dd HH:mm:ss"));

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
                if (oGA.goAutoTask()) action.goManorHarvestAll(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskCycleShop()
        {
            int dow = auto.ScheduleInfo.getGameDOW();
            DateTime now = DateTime.Now;

            // CycleShop only available on Sunday and Wednesday
            if ((dow != 0) && (dow != 3)) return;
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    List<CycleShopInfo> csis = action.goShopGetCycleShopInfo(oGA.currHeader, oGA.sid);
                    foreach (CycleShopInfo csi in csis)
                    {
                        // No way, can only check using this string, or hard code the position.  Text has been converted to TradChinese
                        if ((csi.pos < 3) && (!csi.sold) && ((csi.res == "銀兩") || (csi.res == "银两")))
                        {
                            string info = oGA.msgPrefix + "用銀買 " + csi.nm + " : ";
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
                if (oGA.goAutoTask()) action.goTaskFinishTaskAll(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskSingInAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask()) action.goSignIn(oGA, UpdateInfoHandler);
            }
        }

        private void goTaskReadEmail()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    int emailCnt = action.goEmailReadAll(oGA.currHeader, oGA.sid);
                    if (emailCnt > 0) UpdateResult(oGA.msgPrefix + string.Format("開啟 {0} 封郵件", emailCnt));
                }
            }
        }

        private void goTaskSLBuyFood()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    int buyCount = action.goSLShopBuyFood(oGA);
                    if (buyCount > 0) UpdateResult(oGA.msgPrefix + string.Format("勢力商店買了 {0} 次糧", buyCount));
                }
            }
        }

        private void goTaskCleanBag()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    int cleanUpCount = action.goBagCleanUp(oGA, UpdateInfoHandler);
                }
            }
        }

        private void goTasksIndustryBuyAll(bool buyFood = true, bool buySilver = false)
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    // go buy food first, then silver
                    int buyCnt = 0;

                    if (buyFood) buyCnt = action.goIndustryBuyAll(oGA, null, true, false);

                    // can also buy food if possible
                    if (buySilver) buyCnt += action.goIndustryBuyAll(oGA, null, buyFood, true);

                    if (buyCnt > 0) UpdateResult(oGA.msgPrefix + string.Format("在產業中購買了{0}次", buyCnt));
                }
            }

        }

        private void goTaskNavalWar()
        {
            int dow = auto.ScheduleInfo.getGameDOW();
            DateTime now = DateTime.Now;
            if ((dow != 1) && (dow != 2)) return;
            if ((now.Hour < 9) || (now.Hour > 21)) return;

            int cityId = (dow == 1 ? 1 : 3);
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    bool sendOK = action.sendNavalTroops(oGA, cityId, oGA.BossWarBody);
                    UpdateResult(oGA.msgPrefix + (sendOK ? "跨服入侵準備完成" : "跨服入侵準備失敗"));

                }
            }
        }

        private void goTaskLuckyCycle()
        {
            DateTime now = DateTime.Now;

            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    int drawCount = action.goLuckyCycle(oGA);
                    if (drawCount > 0)
                    {
                        UpdateResult(oGA.msgPrefix + string.Format("轉了{0}次幸運轉盤.", drawCount));
                    }
                }
            }
        }

        private void goTaskTuanGou()
        {
            DateTime now = DateTime.Now;
            int dow = auto.ScheduleInfo.getGameDOW();
            if ((dow != 0) && (dow != 6)) return;

            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goTuanGou(oGA, UpdateInfoHandler);
                }
            }
        }

        private void goTaskShuangShiyiActivityReward()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goShuangShiyiActivityReward(oGA, UpdateInfoHandler);
                }
            }

        }

        private void goTaskBossWar()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    if ((oGA.BossWarBody == null) || (oGA.BossWarBody == ""))
                    {
                        UpdateResult(oGA.msgPrefix + "神將無雙尚未設定");
                    }
                    else
                    {
                        BossWarInfo bwi = action.goBossWarOnce(oGA.currHeader, oGA.sid, oGA.BossWarBody, UpdateInfoHandler);
                        string info = oGA.msgPrefix;
                        if (bwi.enterFail)
                        {
                            info += "進入戰場失敗";
                        }
                        else if (!bwi.bossAvailable)
                        {
                            info += "沒有神將無雙";
                        }
                        else
                        {
                            info += string.Format("神將 HP: {0}, 第 {1} 次出兵: ", bwi.bossHP, bwi.beforeCnt + 1);
                            if (bwi.sendFail)
                            {
                                info += "失敗";

                            }
                            else
                            {
                                info += "成功";
                                if (bwi.leavelFail)
                                {
                                    info += ", 但離開時出錯";
                                }
                            }
                        }

                        UpdateResult(info);
                    }
                }
            }
        }


        private void goTaskEliteBuyTime()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goEliteBuyTime(oGA, UpdateInfoHandler);
                }
            }

        }

        private void goTaskTrialsBuyTimes()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goTrialsBuyTimes(oGA, UpdateInfoHandler);
                }
            }

        }

        private void goTaskTravel()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goTravel(oGA, UpdateInfoHandler);
                }
            }

        }

        private void goTaskGetArenaReward()
        {
            int currHour = DateTime.Now.Hour;

            if (currHour < 21) return;

            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.acceptArenaRankReward(oGA, UpdateInfoHandler);

                    action.drawAreansTimeReward(oGA, UpdateInfoHandler);

                }
            }

        }


        private void goTaskMarketTask()
        {
            int currHour = DateTime.Now.Hour;

            // Only need to try at 6-9.  Good enough to finish
            if ((currHour < 6) || (currHour > 9)) return;

            foreach (GameAccount oGA in gameAccounts)
            {
                if (oGA.goAutoTask())
                {
                    action.goMarketTask(oGA, UpdateInfoHandler);
                }
            }

        }


    }
}
