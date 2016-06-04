using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class auto
    {
        public class TaskInfo
        {
            public string id { get; set; }
            public bool isEnabled { get; set; }
            public ScheduleInfo schedule { get; set;  }
            public dynamic parm   {  get; set; }

            public TaskInfo(string taskId)
            {
                this.id = taskId;
                isEnabled = false;
                schedule = new ScheduleInfo();
                parm = JSON.Empty;
            }

            public bool readyToGo()
            {
                return (isEnabled && schedule.readyToGo());
            }

        }

    }
}
