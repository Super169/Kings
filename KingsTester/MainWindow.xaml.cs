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

        private void btnHeroList_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            List<HeroInfo> heroList = action.Hero_getPlayerHeroList(oGA.currHeader, oGA.sid);
            if (heroList == null) return;

            StringBuilder sb = new StringBuilder();

            foreach(HeroInfo hi in heroList)
            {
                sb.Append(string.Format("{0} : {1} : {2} : {3} : {4} : {5} : {6} : {7} : {8} : {9} : {10} : {11}\n", 
                          hi.idx, hi.nm, hi.army, hi.lv, hi.power, hi.cfd, hi.intl, hi.strg, hi.chrm, hi.attk, hi.dfnc, hi.spd));
            }
            txtResult.Text = sb.ToString();


            gvResult.Columns.Clear();
            addResultColumn("序號", 30, "idx");
            addResultColumn("英雄名稱", 80, "nm");
            addResultColumn("兵種", 60, "army");
            addResultColumn("等級", 30, "lv");
            addResultColumn("戰力", 60, "power");
            lvResult.ItemsSource = heroList;

        }

        private void addResultColumn(string header, int width, string binding)
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


    }
}
