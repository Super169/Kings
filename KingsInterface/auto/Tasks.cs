using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class auto
    {
        public class Tasks
        {
            public static bool AutoClearnBag(ref GameAccount oGA, action.DelegateUpdateInfo updateInfo)
            {
                DateTime now = DateTime.Now;

                TaskInfo ti = oGA.AutoTasks.SingleOrDefault(x => x.id == TID_CLEAN_BAG);
                if (ti == null)
                {

                    ti = new TaskInfo(TID_CLEAN_BAG);
                    // Default setting for TID_CLEAN_BAG, clean the bag every hour (i.e. 60min)
                    ti.elapseMin = 60;
                    ti.startTime = new TimeSpan(6, 5, 0);
                    ti.endTime = new TimeSpan(3, 55, 0);
                    ti.setNextExecutionTime();
                    oGA.AutoTasks.Add(ti);
                }

                if (ti.readyToGo())
                {
                    int cleanUpCount = action.goBagCleanUp(oGA, updateInfo);
                    ti.lastExecutionTime = now;
                    ti.setNextExecutionTime();
                }

                return true;
            }

            public static bool AutoNaval(ref GameAccount oGA, action.DelegateUpdateInfo updateInfo)
            {
                DateTime now = DateTime.Now;

                TaskInfo ti = oGA.AutoTasks.SingleOrDefault(x => x.id == TID_NAVAL_WAR);
                if (ti == null)
                {
                    ti = new TaskInfo(TID_NAVAL_WAR);
                    // Default setting for TID_CLEAN_BAG
                    ti.dow = new List<int>();
                    ti.dow.Add(1);
                    ti.dow.Add(2);
                    ti.startTime = new TimeSpan(9, 15, 0);
                    ti.endTime = new TimeSpan(7, 55, 0);
                    ti.setNextExecutionTime();
                    oGA.AutoTasks.Add(ti);
                }

                if (ti.readyToGo())
                {
                    int cityId = 1;
                    if (((int)now.DayOfWeek) == 2) cityId = 3;
                    // TODO: seting for naval war not ready, use BossWarBody at this moment.
                    bool sendOK = action.sendNavalTroops(oGA, cityId, oGA.BossWarBody);
                }
                return true;
            }
        }
    }
}

