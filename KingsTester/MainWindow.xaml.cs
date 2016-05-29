using Fiddler;
using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static KingsInterface.KingsMonitor;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using MyUtil;

namespace KingsTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<GameAccount> gameAccounts = new ObservableCollection<GameAccount>();
        Object gameAccountsLocker = new Object();

        bool normalMode = true;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "大重帝輔助測試工具  v" + Assembly.GetExecutingAssembly().GetName().Version;

            normalMode = true;
            UpdateInfo("大重帝輔助測試工具 啟動");
            setUI();


            lvAccounts.ItemsSource = gameAccounts;
            // Start Fiddler to retrieve old accounts
            com.start("KingTester");
            restoreAccounts();

            KingsMonitor.notificationEventHandler += new NotificationEventHandler(this.OnNotificationHandler);
            KingsMonitor.newSidEventHandler += new NewSidEventHandler(this.OnNewSidHandler);

            if (!KingsMonitor.Start())
            {
                MessageBox.Show("啟動監察器失敗");
                this.Close();
            }

            autoTimer.Elapsed += new System.Timers.ElapsedEventHandler(autoTimerElapsedEventHandler);
            autoTimer.Enabled = false;
            goAutoKings();
        }

        private void OnNewSidHandler(LoginInfo li, HTTPRequestHeaders oH)
        {

            // UpdateInfo(string.Format("Server: {0}, SID: {1}, {2} - {3}", li.server, li.sid, li.serverTitle, li.nickName));
            UpdateAccountList(li, oH);
        }

        void UpdateAccountList(LoginInfo li, HTTPRequestHeaders oH)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateAccountList(li, oH)));
                return;
            }

            lock (gameAccountsLocker)
            {
                GameAccount oExists = gameAccounts.SingleOrDefault(x => x.account == li.account);
                if (oExists == null)
                {
                    GameAccount oGA = new GameAccount(li, oH);
                    // RestoreProfile(oGA);
                    gameAccounts.Add(oGA);
                    if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;
                    UpdateInfo(String.Format("加入 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
                else
                {
                    oExists.sid = li.sid;
                    oExists.serverTitle = li.serverTitle;
                    oExists.nickName = li.nickName;
                    oExists.currHeader = oH;
                    oExists.lastUpdateDTM = DateTime.Now;
                    oExists.status = AccountStatus.Online;
                    oExists.setInfo();
                    refreshAccountList();
                    UpdateInfo(String.Format("更新 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
            }
        }

        void UpdateAccountList(GameAccount oGA)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateAccountList(oGA)));
                return;
            }

            lock (gameAccountsLocker)
            {
                GameAccount oExists = gameAccounts.SingleOrDefault(x => x.account == oGA.account);
                if (oExists != null) gameAccounts.Remove(oExists);
                gameAccounts.Add(oGA);
            }
            if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;

        }



        void refreshAccountList()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => refreshAccountList()));
                return;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);
            view.Refresh();
        }


        private void OnNotificationHandler(string info)
        {
            // UpdateInfo(info);
        }

        private void UpdateTextBox(TextBox tb, string content, bool async = true)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                if (async)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateTextBox(tb, content, async)));
                } else
                {
                    Application.Current.Dispatcher.Invoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateTextBox(tb, content, async)));
                }
                return;
            }
            tb.Text = content;
            tb.ScrollToEnd();
        }

        private void UpdateUI(TextBox tb, string info, bool addTime = true, bool resetText = false, bool newLine = true)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateUI(tb, info, false, resetText, newLine)));
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ");
            tb.Text += info + (newLine ? "\n" : "");
            tb.ScrollToEnd();

        }

        private void UpdateInfo(string info, bool addTime = true, bool resetText = false)
        {
            UpdateUI(txtInfo, info, addTime, resetText);
        }

        private void UpdateResult(string info, bool addTime = true, bool resetText = false)
        {
            UpdateUI(txtResult, info, addTime, resetText);
        }

        private void AddResultColumn(string header, int width, string binding)
        {
            GridViewColumn gvc = new GridViewColumn();
            gvc.Header = header;
            gvc.Width = width;
            gvc.DisplayMemberBinding = new Binding(binding);

            FrameworkElementFactory fef = new FrameworkElementFactory(typeof(TextBlock));
            DataTemplate dt = new DataTemplate();
            fef.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);

            Binding bd = new Binding();
            fef.SetValue(TextBlock.TextProperty, bd);
            fef.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Right);
            dt.VisualTree = new FrameworkElementFactory(typeof(Grid));
            dt.VisualTree.AppendChild(fef);
            gvc.HeaderTemplate = dt;

            gvResult.Columns.Add(gvc);
        }

        private void btnGoAction_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            string sAction = cboAction.Text.Split('|')[0].Trim();
            string sBody = null;
            switch (sAction)
            {
                case "Activity.drawCompanyAnniversaryLoginReward":
                case "Activity.drawCompanyAnniversaryRechargeReward":
                case "Activity.drawExchangeHoliday":
                case "Activity.drawStrategicFundInfo":
                case "Activity.getActivityList":
                case "Activity.getAnnouncement":
                case "Activity.getBookHeroInfo":
                case "Activity.getCloudSellerInfo":
                case "Activity.getPlayerGoBackActivityInfo":
                case "Activity.getRankInfo":
                case "Activity.getRationActivity":
                case "Activity.getShuangShiyiActivityInfo":
                case "Activity.getShuangShiyiActivityReward":
                case "Activity.getTuanGouInfo":
                case "Activity.getTuanGouOpenInfo":
                case "Activity.serverOpenTime":
                case "Activity.showIconForServerOpenActivity":
                case "Bag.getBagInfo":
                case "BaiGuanYu.getTimeInfo":
                case "BossWar.bossInfo":
                case "BossWar.bossLineup":
                case "BossWar.enterWar":
                case "BossWar.keyKillInfo":
                case "BossWar.leaveWar":
                case "BossWar.openInfo":
                case "BossWar.pk":
                case "BossWar.rankInfo":
                case "Campaign.eliteBuyTime":
                case "Campaign.eliteGetAllInfos":
                case "Campaign.fightNext":
                case "Campaign.getLeftTimes":
                case "Campaign.getTrialsInfo":
                case "Campaign.nextEnemies":
                case "Campaign.quitCampaign":
                case "ChristmasTree.time":
                case "Circle.challenge":
                case "Circle.drawPassRewards":
                case "Circle.getHuarongRoadInfo":
                case "Circle.restartHuarongRoad":
                case "City.getPlayerCityInfo":
                case "Corps.corpsCityReward":
                case "Corps.getCorpsMessageNum":
                case "Corps.getJoinedCorps":
                case "Corps.personIndustryRefresh":
                case "Corps.playerTech":
                case "Corps.takeZhanjiStep":
                case "CorpsWar.getInfo":
                case "Country.corpsCountry":
                case "Country.viewCountry":
                case "CrossCamp.getInfo":
                case "CrossCamp.getJackpotInfo":
                case "Drama.getDramaInfo":
                case "EightTrigrams.getInfo":
                case "EightTrigrams.open":
                case "EightTrigrams.openBox":
                case "Email.openInBox":
                case "Emperor.collected":
                case "Emperor.draw":
                case "Emperor.getBuyInfo":
                case "Emperor.getGameInfo":
                case "Emperor.isFloat":
                case "Firecracker.activityInfo":
                case "Firecracker.myFirecrackerInfo":
                case "Gamble.chouqianOpenInfo":
                case "GmActivity.superPackageInfo":
                case "God.godStrenInfo":
                case "GrassArrow.acquireGrassArrowInfo":
                case "GrassArrow.doGrassArrowFight":
                case "Hero.assessScore":
                case "Hero.getConvenientFormations":
                case "Hero.getCurrRecommendActivityInfo":
                case "Hero.getFeastInfo":
                case "Hero.getHeroIconInfo":
                case "Hero.getPlayerHeroList":
                case "Hero.getWineInfo":
                case "KingRoad.afterSeasonEnemy":
                case "KingRoad.kingroadEnd":
                case "KingRoad.kingroadState":
                case "Login.getOfflineConpensate":
                case "Login.loginFinish":
                case "Login.serverInfo":
                case "LongMarch.getBuyResetTimes":
                case "LongMarch.getFinishedReward":
                case "LongMarch.getHeroStatus":
                case "LongMarch.getMyStatus":
                case "LongMarch.getUnpassBuff":
                case "LongMarch.restart":
                case "LordGodUp.getDispInfo":
                case "Lottery.drawLottery":
                case "Lottery.freeLottery":
                case "Lottery.openLottery":
                case "Lottery.refreshLottery":
                case "LuckyCycle.draw":
                case "LuckyCycle.info":
                case "Major.getMyMajorInfo":
                case "Manor.armsTechnology":
                case "Manor.decreeInfo":
                case "Manor.getManorInfo":
                case "Manor.refreshManor":
                case "Manor.resHourOutput":
                case "MonthSignIn.getOpenInfo":
                case "NationalWar.acquireCityCommandInfo":
                case "NationalWar.acquireNationCardPanelInfo":
                case "NationalWar.getMyTroops":
                case "Naval.getInfo":
                case "Naval.inMissionHeros":
                case "Naval.killRank":
                case "Naval.leaveWar":
                case "Naval.rewardCfg":
                case "NorthMarch.enterWar":
                case "NorthMarch.inMissionHeros":
                case "NorthMarch.leaveWar":
                case "NorthMarch.northCitySituation":
                case "NorthMarch.retreatAllTroops":
                case "Notice.queryAllMarqueeMessage":
                case "NpcCorps.getNpcWars":
                case "OneYear.cityStatus":
                case "Patrol.getPatrolInfo":
                case "Platform.getPlatformInfo":
                case "Player.getProperties":
                case "Player.getSpecialState":
                case "Pray.getPrayTime":
                case "Rank.findAllPowerRank":
                case "RedEnvelope.activityTime":
                case "RewardActivity.getSevenDayFundRewardInfo":
                case "Shop.availableShops":
                case "Shop.getCycleShopInfo":
                case "Shop.getResourceTradeInfo":
                case "Shop.getShuangShiyiShopInfo":
                case "Shop.getTravelShopInfo":
                case "Shop.otherShopsRefreshTime":
                case "Shop.shopNextRefreshTime":
                case "Shop2.availableShops":
                case "SignInReward.getInfo":
                case "SignInReward.signIn":
                case "StarGazing.activityInfo":
                case "StarGazing.myFirecrackerInfo":
                case "Task.getAchievementInfo":
                case "Task.getTaskTraceInfo":
                case "TeamDuplicate.teamDuplicateFreeTimes":
                case "Travel.checkOut":
                case "Travel.dice":
                case "Travel.escape":
                case "Travel.getMapInfo":
                case "Travel.getStatus":
                case "Travel.restartTravel":
                case "TurnCardReward.getTurnCardRewards":
                case "Valentine.getActivityInfo":
                case "Vip.firstChargeInfo":
                case "Vip.monthCard":
                case "VipAuthentication.isGotMobileGift":
                case "WelfareLottery.time":
                case "World.getAllOpenedCities":
                case "World.getAllTransportingUnits":
                case "World.getCityChapterBlueprint":
                case "World.getCityRewardInfo":
                case "World.getExploredWorldArea":
                case "World.worldSituation":
                case "YunYou.getYunYouInfo":
                case "ZaJinDan.getTimeInfo":

                case "TeamDuplicate.battleStart":
                case "TeamDuplicate.duplicateList":
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction);
                    break;
                case "Naval.enterWar":
                    sBody = "{\"n\":2}";
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction, true, sBody);
                    break;
                case "Archery.getArcheryInfo":
                    sBody = "{\"type\":\"NORMAL\"}";
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction, true, sBody);
                    break;
                case "Login.login":
                    sBody = "{\"type\":\"WEB_BROWSER\",\"loginCode\":\"" + oGA.sid + "\"}";
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction, false, sBody);
                    break;
                case "System.ping":
                    TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                    Int64 jsTime = (Int64)(t.TotalMilliseconds + 0.5);
                    sBody = "{\"clientTime\":\"" + jsTime.ToString() + " \"}";
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction, true, sBody);
                    break;
                default:
                    UpdateResult(string.Format("指令 {0} 尚未支援", sAction));
                    break;
            }
        }

        private void ShowActionResult(string result)
        {
            if (cbxCleanUp.IsChecked == true) result = com.CleanUpResponse(result);
            UpdateResult(result);
        }


        private void ShowGenericActionResult(HTTPRequestHeaders oH, string sid, string command, bool addSId = true, string body = null)
        {
            RequestReturnObject rro = com.SendGenericRequest(oH, sid, command, addSId, body);
            if (rro.success)
            {
                ShowActionResult(rro.responseText);
            }
            else
            {
                ShowActionResult(rro.msg);
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            action.goSignIn(oGA, UpdateInfoHandler);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            txtResult.TextWrapping = TextWrapping.Wrap;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtResult.TextWrapping = TextWrapping.NoWrap;
        }

        private void UpdateInfoHandler(string info)
        {
            UpdateUI(txtResult, info);
        }

        private void btnArchery_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            action.goArcheryShootAll(oGA.currHeader, oGA.sid, UpdateInfoHandler);
        }

        private void btnPlayerInfo_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            PlayerProperties pp = action.goGetPlayerProperties(oGA.currHeader, oGA.sid);
            if (pp.ready)
            {
                UpdateResult("主公資料:");
                UpdateResult(string.Format("LEVEL: {0}; VIP_LEVEL: {1}", pp.LEVEL, pp.VIP_LEVEL));
                UpdateResult(string.Format("EXP: {0}; UNDERGO_EXP: {1}; LEVEL_UP_EXP: {2}", pp.EXP, pp.UNDERGO_EXP, pp.LEVEL_UP_EXP));
                UpdateResult(string.Format("GOLD: {0}; LONGMARCH_COIN: {1}; CSKING_COIN: {2}; GOLD_TICKET: {3}", pp.GOLD, pp.LONGMARCH_COIN, pp.CSKING_COIN, pp.GOLD_TICKET));
                UpdateResult(string.Format("SILVER: {0}/{1}; FOOD: {2}/{3}; IRON: {4}/{5}", pp.SILVER, pp.MAX_SILVER, pp.FOOD, pp.MAX_FOOD, pp.IRON, pp.MAX_IRON));
            }
            else
            {
                UpdateResult("讀取主公資料出錯");
            }
        }

        private void btnManorInfo_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            List<ManorInfo> mis = action.goManorGetManorInfo(oGA.currHeader, oGA.sid);
            UpdateResult("封田資料:");
            foreach (ManorInfo mi in mis)
            {
                UpdateResult(string.Format("field: {0}; type: {1}; level: {2}; levelSeconds: {3}; heroIndex: {4}; leftSeconds: {5}; products: {6}; produceSeconds: {7}",
                                           mi.field, mi.type, mi.level, mi.levelSeconds, mi.heroIndex, mi.leftSeconds, mi.products, mi.produceSeconds));
            }
        }

        private void btnHarvestAll_Click(object sender, RoutedEventArgs e)
        {
            goTaskHarvestAll();
        }

        private GameAccount GetSelectedActiveAccount(bool allowOffline = false)
        {
            if (gameAccounts.Count == 0)
            {
                MessageBox.Show("尚未偵測到大皇帝帳戶, 請先登入遊戲.");
                return null;
            }

            GameAccount oGA = (GameAccount)lvAccounts.SelectedItem;
            if (oGA == null) MessageBox.Show("請先選擇帳戶");

            if (oGA.CheckStatus() != AccountStatus.Online)
            {
                if (!allowOffline)
                {
                    MessageBox.Show("帳戶已在其他地方登入");
                    return null;
                }
            }
            return oGA;
        }

        private void btnHeroList_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount(true);
            if (oGA == null) return;

            bool onlineData = (oGA.CheckStatus(true) == AccountStatus.Online);

            if (onlineData)
            {
                oGA.Heros.Clear();

                oGA.Heros = action.goGetHerosInfo(oGA.currHeader, oGA.sid);
                if (oGA.Heros == null) return;
            }

            StringBuilder sb = new StringBuilder();
            if (!onlineData) sb.Append("#### 帳戶已在其他地方登入, 以下顯示資料可能已過時 ####\n");
            sb.Append("Hero List:\n");

            foreach (HeroInfo hi in oGA.Heros)
            {
                sb.Append(string.Format("{0} : {1} : {2} : {3} : {4} : {5} : {6} : {7} : {8} : {9} : {10} : {11}\n",
                          hi.idx, hi.nm, hi.army, hi.lv, hi.power, hi.cfd, hi.intl, hi.strg, hi.chrm, hi.attk, hi.dfnc, hi.spd));
            }
            UpdateResult(sb.ToString());

            gvResult.Columns.Clear();
            AddResultColumn("序號", 30, "idx");
            AddResultColumn("英雄名稱", 80, "nm");
            AddResultColumn("兵種", 60, "army");
            AddResultColumn("等級", 30, "lv");
            AddResultColumn("戰力", 60, "power");
            lvResult.ItemsSource = oGA.Heros;

        }

        private void btnDecInfo_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount(true);
            if (oGA == null) return;

            bool onlineData = (oGA.CheckStatus(true) == AccountStatus.Online);

            if (onlineData)
            {
                oGA.DecreeHeros.Clear();

                if (oGA.Heros.Count == 0) oGA.Heros = action.goGetHerosInfo(oGA.currHeader, oGA.sid);
                // Fail to get hero info
                if (oGA.Heros.Count == 0)
                {
                    UpdateResult("讀取英雄資料失敗");
                    return;
                }

                oGA.DecreeHeros = action.goManorGetDecreeInfoWithName(oGA.currHeader, oGA.sid, oGA.Heros);

            }
            else
            {
                UpdateResult("#### 帳戶已在其他地方登入, 以下顯示資料可能已過時 ####");
            }

            foreach (DecreeInfo di in oGA.DecreeHeros)
            {
                UpdateResult(string.Format("{0}: [{1}] [{2}] [{3}] [{4}] [{5}]", di.name(), di.heroName[0], di.heroName[1], di.heroName[2], di.heroName[3], di.heroName[4]));
            }
        }

        private void btnBossWarSetting_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedActiveAccount(true);
            if (oGA == null) return;

            bool onlineData = (oGA.CheckStatus(true) == AccountStatus.Online);

            if (onlineData)
            {
                if (oGA.Heros.Count == 0) oGA.Heros = action.goGetHerosInfo(oGA.currHeader, oGA.sid);
                // Fail to get hero info
                if (oGA.Heros.Count == 0)
                {
                    UpdateResult("讀取英雄資料失敗");
                    return;
                }
            }
            else
            {
                if (oGA.Heros.Count == 0)
                {
                    UpdateResult("#### 帳戶已在其他地方登入, 沒有英雄資料可用");
                    return;
                }
            }

            ui.BossWarSettings Window = new ui.BossWarSettings();
            Window.Owner = this;
            Window.Title = "神將無雙佈陣";
            Window.setData(oGA);
            bool? dialogResult = Window.ShowDialog();
            if (dialogResult == true)
            {
                refreshAccountList();
                // SaveProfile();
                saveAccounts();
            }
        }

        private void btnBossWar_Click(object sender, RoutedEventArgs e)
        {
            GoBossWarOnce();
        }

        private void btnCycleShop_Click(object sender, RoutedEventArgs e)
        {
            goTaskCycleShop();
        }

        private void setUI()
        {
            btnArchery.IsEnabled = normalMode;
            btnBossWar.IsEnabled = normalMode;
            // can always change BossWar setting
            btnBossWarSetting.IsEnabled = true;
            btnCycleShop.IsEnabled = normalMode;
            btnDecInfo.IsEnabled = normalMode;
            cboAction.IsEnabled = normalMode;
            btnGoAction.IsEnabled = normalMode;
            btnGoAuto.IsEnabled = true;
            btnGoAuto.Content = (normalMode ? "啟動" : "停止") + " 自動大皇帝";
            btnGoAuto.Background = (normalMode ? btnArchery.Background : Brushes.Red);
            btnHarvestAll.IsEnabled = normalMode;
            btnHeroList.IsEnabled = normalMode;
            btnManorInfo.IsEnabled = normalMode;
            btnPlayerInfo.IsEnabled = normalMode;
            btnReadMail.IsEnabled = normalMode;
            btnSignIn.IsEnabled = normalMode;
            lblAutoRunning.Content = (normalMode ? "自動大皇帝 準備中" : "自動大皇帝 已啟動");
            lblAutoRunning.Background = (normalMode ? Brushes.Gray : new SolidColorBrush(Color.FromArgb(255, 63, 255, 0)));
            lblAutoRunning.Foreground = (normalMode ? Brushes.Black : Brushes.Red);
            // lblAutoRunning.Visibility = (normalMode ? Visibility.Hidden : Visibility.Visible);
            lblAutoRunning2.Content = (normalMode ? "獨立測試" : "自動大皇帝 已啟動");
            lblAutoRunning2.Background = (normalMode ? Brushes.LightBlue : Brushes.LightGreen);
            lblAutoRunning2.Foreground = (normalMode ? Brushes.Red : Brushes.Black);

            // lblAutoRunning2.Visibility = (normalMode ? Visibility.Hidden : Visibility.Visible);
        }

        private void btnGoAuto_Click(object sender, RoutedEventArgs e)
        {
            goAutoKings();
        }


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

        private void btnReadMail_Click(object sender, RoutedEventArgs e)
        {
            goTaskReadEmail();
        }


        private void btnSaveAccounts_Click(object sender, RoutedEventArgs e)
        {
            saveAccounts();
        }

        private void saveAccounts()
        {
            List<GFR.GenericFileRecord> gfrs = new List<GFR.GenericFileRecord>();

            foreach (GameAccount oGA in gameAccounts)
            {
                gfrs.Add(oGA.toGenericFileRecord());
            }

            GFR.saveGFR("GFR.DAT", gfrs);
        }

        private void btnReadAccounts_Click(object sender, RoutedEventArgs e)
        {
            restoreAccounts();
        }

        private void restoreAccounts()
        {
            List<GFR.GenericFileRecord> gfrs = null;
            if (GFR.restoreGFR("GFR.DAT", ref gfrs))
            {
                lock (gameAccountsLocker)
                {
                    gameAccounts.Clear();
                    refreshAccountList(gfrs);
                }
            }
        }

        private void refreshAccountList(List<GFR.GenericFileRecord> gfrs)
        {
            int currSelectedIndex = lvAccounts.SelectedIndex;

            lock (gameAccountsLocker)
            {
                gameAccounts.Clear();
                foreach (GFR.GenericFileRecord gfr in gfrs)
                {
                    gameAccounts.Add(new GameAccount(gfr));
                }
            }
            lvAccounts.SelectedIndex = (currSelectedIndex == -1 ? 0 : currSelectedIndex);
            goCheckAccountStatus(true);
            foreach (GameAccount oGA in gameAccounts)
            {
                KingsMonitor.addAccount(oGA.account, oGA.sid);
            }
            refreshAccountList();

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!normalMode) goAutoKings();
            saveAccounts();
        }

        private void btnCheckStatus_Click(object sender, RoutedEventArgs e)
        {
            goCheckAccountStatus();
        }


        private void btnSaveResult_Click(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Log");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                if (!Directory.Exists(filePath)) filePath = Directory.GetCurrentDirectory();
            }
            string fileName = string.Format("{0:yyyyMMddHHmm}.Log", DateTime.Now);
            string fullName = System.IO.Path.Combine(filePath, fileName);

            File.WriteAllText(fullName, txtResult.Text);
            MessageBox.Show(string.Format("Result saved to {0}", fullName));
        }

        private void btnWorking_Click(object sender, RoutedEventArgs e)
        {
            kingsWorkingTester();
        }

        private void btnReloadAll_Click(object sender, RoutedEventArgs e)
        {
            reloadAll();
        }
    }
}
