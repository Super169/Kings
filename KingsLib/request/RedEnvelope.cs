using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class RedEnvelope
    {
        private const string CMD_activityTime = "RedEnvelope.activityTime";

        public static RequestReturnObject activityTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_activityTime);
        }


    }
}
