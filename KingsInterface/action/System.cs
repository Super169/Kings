using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static long getSystemTime(HTTPRequestHeaders oH, string sid)
        {
            long currTime = 0;
            RequestReturnObject rro = action.go_System_ping(oH, sid);
            if (rro.success && rro.Exists("serverTime"))
            {
                currTime = JSON.getLong(rro.responseJson, "serverTime");
            } else
            {
                TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                currTime = (long)(t.TotalMilliseconds + 0.5);
            }
            return currTime;
        }

    }
}
