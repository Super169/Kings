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
                    ti = new TaskInfo() { id = TID_CLEAN_BAG };
                    // Default setting for TID_CLEAN_BAG
                    ti.dow = new List<int>();
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
        }
    }
}

