using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static List<TaskTraceInfo> goTaskGetTaskTraceInfo(HTTPRequestHeaders oH, string sid)
        {
            List<TaskTraceInfo> ttis = new List<TaskTraceInfo>();
            try
            {
                RequestReturnObject rro = go_Task_getTaskTraceInfo(oH, sid);
                if ((!rro.success) || (rro.responseJson == null) || (rro.responseJson["tasks"] == null)) return ttis;

                DynamicJsonArray tasks = rro.responseJson["tasks"];
                foreach (dynamic task in tasks)
                {
                    TaskTraceInfo tti = new TaskTraceInfo();
                    tti.id = getInt(task, "id");
                    tti.status = task["status"];
                    tti.done = (tti.status == "FIN");
                    ttis.Add(tti);
                }
            }
            catch (Exception) { }
            return ttis;
        }

        public static int goTaskFinishTaskAll(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            int finCount = 0;
            List<TaskTraceInfo> ttis = goTaskGetTaskTraceInfo(oGA.currHeader, oGA.sid);
            if (ttis.Count == 0) return 0;

            foreach (TaskTraceInfo tti in ttis)
            {
                if (tti.done)
                {
                    RequestReturnObject rro = action.go_Task_finishTask(oGA.currHeader, oGA.sid, tti.id);
                    if ((rro.success) && (rro.responseJson != null) && (rro.responseJson["taskId"] !=null))
                    {
                        int rroTaskId = getInt(rro.responseJson, "taskId", -1);
                        if (rroTaskId == tti.id) finCount++;
                    }
                }
            }
            if ((finCount > 0) && (updateInfo != null))
            {
                updateInfo(oGA.msgPrefix() + string.Format("領取 {0} 項任務報酬)", finCount));
            }
            return finCount;
        }
    }

}
