﻿using Fiddler;
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

            cboAction.IsEnabled = true;
            btnGoAction.IsEnabled = true;

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

        private void UpdateUI(TextBox tb, string info, bool addTime = true, bool resetText = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateUI(tb, info, false, resetText)));
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss | ");
            tb.Text += info + "\n";
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
                case "Player.getProperties":
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
            } else
            {
                ShowActionResult(rro.msg);
            }
        }

        private void btnHeroList_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            List<HeroInfo> heroList = action.goGetHerosInfo(oGA.currHeader, oGA.sid);
            if (heroList == null) return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Hero List:\n");

            foreach (HeroInfo hi in heroList)
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
            lvResult.ItemsSource = heroList;

        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            action.goSignIn(oGA.currHeader, oGA.sid, UpdateInfoHandler);
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
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            action.goArcheryShootAll(oGA.currHeader, oGA.sid, UpdateInfoHandler);
        }

        private void btnPlayerInfo_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            PlayerProperties pp = action.goGetPlayerProperties(oGA.currHeader, oGA.sid);
        }
    }
}
