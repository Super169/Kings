using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Task
    {
        private const string CMD_finishTask = "Task.finishTask";
        private const string CMD_getAchievementInfo = "Task.getAchievementInfo";
        private const string CMD_getTaskTraceInfo = "Task.getTaskTraceInfo";

        public static RequestReturnObject getAchievementInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getAchievementInfo);
        }

        public static RequestReturnObject getTaskTraceInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTaskTraceInfo);
        }


    }
}
