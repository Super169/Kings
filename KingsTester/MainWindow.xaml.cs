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

namespace KingsTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<GameAccount> gameAccounts = new ObservableCollection<GameAccount>();
        Object gameAccountsLocker = new Object();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "大重帝輔助測試工具  v" + Assembly.GetExecutingAssembly().GetName().Version;
            KingsMonitor.notificationEventHandler += new NotificationEventHandler(this.OnNotificationHandler);
            KingsMonitor.newSidEventHandler += new NewSidEventHandler(this.OnNewSidHandler);

            if (!KingsMonitor.Start("KingsTester"))
            {
                MessageBox.Show("啟動監察器失敗");
                this.Close();
            }
            lvAccounts.ItemsSource = gameAccounts;

            cboAction.IsEnabled = false;
            btnGoAction.IsEnabled = false;

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
                    gameAccounts.Add(new GameAccount(li, oH));
                    if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;
                    UpdateInfo(String.Format("加入 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
                else
                {
                    oExists.sid = li.sid;
                    oExists.currHeader = oH;
                    oExists.lastUpdateDTM = DateTime.Now;
                    refreshAccountList();
                    UpdateInfo(String.Format("更新 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
            }
        }


        void refreshAccountList()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);
            view.Refresh();
        }


        private void OnNotificationHandler(string info)
        {
            // UpdateInfo(info);
        }

        private void UpdateInfo(string info, bool addTime = true, bool resetText = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateInfo(info, false, resetText)));
                return;
            }
            if (resetText) txtInfo.Text = "";
            if (addTime) txtInfo.Text += DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss | ");
            txtInfo.Text += info + "\n";
            txtInfo.ScrollToEnd();
        }


        private GameAccount GetSelectedAccount()
        {
            if (gameAccounts.Count == 0)
            {
                MessageBox.Show("尚未偵測到大皇帝帳戶, 請先登入遊戲.");
                return null;
            }

            GameAccount oGA = (GameAccount)lvAccounts.SelectedItem;
            if (oGA == null) MessageBox.Show("請先選擇帳戶");
            return oGA;
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
            txtResult.Text = "";
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            string sAction = cboAction.Text.Split('|')[0].Trim();
            string sBody = null;
            switch (sAction)
            {
                case "BossWar.bossInfo":
                case "BossWar.enterWar":
                case "BossWar.leaveWar":
                case "BossWar.pk":
                case "Campaign.eliteBuyTime":
                case "Campaign.fightNext":
                case "Campaign.getLeftTimes":
                case "Campaign.getTrialsInfo":
                case "Campaign.nextEnemies":
                case "Campaign.quitCampaign":
                case "Email.openInBox":
                case "Hero.getFeastInfo":
                case "Hero.getConvenientFormations":
                case "Hero.getPlayerHeroList":
                case "Login.serverInfo":
                case "Manor.decreeInfo":
                case "Manor.getManorInfo":
                case "Patrol.getPatrolInfo":
                case "Rank.findAllPowerRank":
                case "Shop.shopNextRefreshTime":
                case "SignInReward.getInfo":
                case "SignInReward.signIn":
                case "TeamDuplicate.battleStart":
                case "TeamDuplicate.duplicateList":
                case "TeamDuplicate.teamDuplicateFreeTimes":
                case "TurnCardReward.getTurnCardRewards":
                case "World.getAllTransportingUnits":
                case "World.worldSituation":
                    ShowGenericActionResult(oGA.currHeader, oGA.sid, sAction);
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
                    txtResult.Text = string.Format("指令 {0} 尚未支援", sAction);
                    break;
            }
        }

        private void ShowActionResult(string result)
        {
            if (cbxCleanUp.IsChecked == true) result = com.CleanUpResponse(result);
            txtResult.Text = result;
        }


        private void ShowGenericActionResult(HTTPRequestHeaders oH, string sid, string command, bool addSId = true, string body = null)
        {
            RequestReturnObject rro = com.SendGenericRequest(oH, sid, command, addSId, body);
            if (rro.success)
            {
                ShowActionResult(rro.responseText);
            } else
            {
                ShowActionResult(rro.msg);
            }
        }

        private void btnHeroList_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            List<HeroInfo> heroList = action.goGetHerosInfo(oGA.currHeader, oGA.sid);
            if (heroList == null) return;

            StringBuilder sb = new StringBuilder();

            foreach (HeroInfo hi in heroList)
            {
                sb.Append(string.Format("{0} : {1} : {2} : {3} : {4} : {5} : {6} : {7} : {8} : {9} : {10} : {11}\n",
                          hi.idx, hi.nm, hi.army, hi.lv, hi.power, hi.cfd, hi.intl, hi.strg, hi.chrm, hi.attk, hi.dfnc, hi.spd));
            }
            txtResult.Text = sb.ToString();


            gvResult.Columns.Clear();
            AddResultColumn("序號", 30, "idx");
            AddResultColumn("英雄名稱", 80, "nm");
            AddResultColumn("兵種", 60, "army");
            AddResultColumn("等級", 30, "lv");
            AddResultColumn("戰力", 60, "power");
            lvResult.ItemsSource = heroList;

        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            txtResult.Text = action.goSignIn(oGA.currHeader, oGA.sid);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            txtResult.TextWrapping = TextWrapping.Wrap;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtResult.TextWrapping = TextWrapping.NoWrap;
        }

        private void btnArchery_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            ArcheryInfo ai = null;
            if (!action.goArcheryShoot(oGA.currHeader, oGA.sid,  ref ai))
            {
                if (ai.returnCode ==  AiReturnCode.NO_ACTIVITY)
                {
                    txtResult.Text += "今天沒有百步穿楊";
                }
                else if (ai.returnCode == AiReturnCode.COMPLETED)
                {
                    txtResult.Text += string.Format("已經再沒有箭可以射了.  現有: {0} 環\n", ai.tRing);
                }
                else
                {
                    txtResult.Text += ai.msg + "\n";
                }
                return;
            }

            txtResult.Text += string.Format("{0}環; {1} 次; 風力: {2}; ({3},{4}) => ({5},{6}) : {7}環\n", 
                                            ai.tRing, ai.arr, ai.wind, ai.goX, ai.goY, ai.atX, ai.atY, ai.ring);
            if (ai.arr == 0)
            {
                txtResult.Text += "已經再沒有箭可以射了\n";
                return;
            }
            goArcheryShoot(oGA, ai);
            txtResult.Text += "\n\n";

        }


        private bool goArcheryShoot(GameAccount oGA, ArcheryInfo ai)
        {
            ai.success = false;
            RequestReturnObject rro;
            string sAction = "Archery.shoot";

            int x, y;
            x = (Math.Abs(ai.wind) < 100 ? 0 : (ai.wind < 0 ? (ai.wind + 100) / -10 : (100 - ai.wind) / 10));
            y = 11;
            txtResult.Text += string.Format("瞄準位置: ( {0} , {1} )\n", x, y);
            ai.body = "{\"x\":" + x.ToString() + ",\"y\":" + y.ToString() + ",\"type\":\"NORMAL\"}";
            // txtResult.Text += "Go " + sAction + "\n" + ai.body + "\n";

            rro = com.SendGenericRequest(oGA.currHeader, oGA.sid, sAction, true, ai.body);
            if (!rro.success)
            {
                txtResult.Text += "\n執行射擊失敗:\n" + rro.msg;
                return false;
            }

            try
            {
                if (rro.responseJson == null)
                {
                    ai.msg = "讀取穿擊結果失敗\n資料空白";
                    return false;
                }

                ai.atX = rro.responseJson.x;
                ai.atY = rro.responseJson.y;
                ai.ring = rro.responseJson.ring;
                ai.nWind = rro.responseJson.nWind;
                txtResult.Text += string.Format("擊中: ( {0} , {1} ), 取得 {2} 環, 下次風力為 {3}", ai.atX, ai.atY, ai.ring, ai.nWind);
                ai.success = true;
            }
            catch (Exception ex)
            {
                ai.msg = "讀取穿擊結果失敗\n" + ex.Message;
                ai.msg += "\n\n" + rro.responseText;
            }

            return ai.success;
        }

    }
}
