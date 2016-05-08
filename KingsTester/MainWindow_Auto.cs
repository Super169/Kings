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

    }
}
