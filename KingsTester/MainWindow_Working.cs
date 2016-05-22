using KingsInterface;
using KingsInterface.data;
using KingsInterface.request;
using MyUtil;
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
            goReward();
        }

        private void goTravel()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            if (!oGA.IsOnline()) return;
            action.goTravel(oGA, UpdateInfoHandler);
        }

        private void goLuckyCycle()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            RequestReturnObject rro;
            rro = LuckyCycle.info(oGA.currHeader, oGA.sid);
            if (!rro.SuccessWithJson("remainCount")) return;
            int remainCount = JSON.getInt(rro.responseJson, "remainCount");
            while (remainCount > 0)
            {
                rro = LuckyCycle.draw(oGA.currHeader, oGA.sid);
                if (!rro.SuccessWithJson("id")) break;
                remainCount--;
                rro = Player.getProperties(oGA.currHeader, oGA.sid);
            }
        }

        private void goReward()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            RequestReturnObject rro;
            rro = Activity.getShuangShiyiActivityReward(oGA.currHeader, oGA.sid);
            if (rro.ok == 1)
            {
                rro = Activity.drawCompanyAnniversaryRechargeReward(oGA.currHeader, oGA.sid);
                if (rro.success)
                {
                    UpdateResult(string.Format("{0}領取紅包", oGA.msgPrefix()));
                }
            }

        }

        private void go_TestAuto()
        {
            auto.ScheduleInfo schedule = new auto.ScheduleInfo();
            /*
            schedule.startTime = new TimeSpan(10, 15, 0);
            schedule.endTime = new TimeSpan(23, 0, 0);
            schedule.dow.Add(1);
            schedule.dow.Add(2);
            schedule.executionTimes.Add(new TimeSpan(5, 25, 0));
            schedule.executionTimes.Add(new TimeSpan(9, 25, 0));
            schedule.executionTimes.Add(new TimeSpan(12,25, 0));
            schedule.executionTimes.Add(new TimeSpan(18, 25, 0));
            */
            dynamic json = schedule.toJson();
            UpdateResult(json.ToString());
            string jsonString = schedule.toJsonString();
            UpdateResult(jsonString);
            schedule.fromJson(json);
            schedule.fromJsonString(jsonString);
        }

        private void go_TestNavalWar()
        {
            GameAccount oGA = GetSelectedActiveAccount();
            if (oGA == null) return;
            bool sendOK = action.sendNavalTroops(oGA, 3, oGA.BossWarBody);
            UpdateResult(oGA.msgPrefix() + (sendOK ? "跨服入侵準備完成" : "跨服入侵準備失敗"));
        }
    }
}
