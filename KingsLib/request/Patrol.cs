using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Patrol
    {
        private const string CMD_dealPatroledEvent = "Patrol.dealPatroledEvent";
        private const string CMD_getPatrolInfo = "Patrol.getPatrolInfo";

        public static RequestReturnObject getPatrolInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPatrolInfo);
        }


    }
}
