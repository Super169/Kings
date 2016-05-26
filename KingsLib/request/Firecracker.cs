using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Firecracker
    {
        private const string CMD_activityInfo = "Firecracker.activityInfo";
        private const string CMD_myFirecrackerInfo = "Firecracker.myFirecrackerInfo";

        public static RequestReturnObject activityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_activityInfo);
        }

        public static RequestReturnObject myFirecrackerInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_myFirecrackerInfo);
        }


    }
}
