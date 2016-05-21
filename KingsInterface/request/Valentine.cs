using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Valentine
    {
        private const string CMD_getActivityInfo = "Valentine.getActivityInfo";
        public static RequestReturnObject getActivityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getActivityInfo);
        }

    }
}
