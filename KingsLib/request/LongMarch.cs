using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class LongMarch
    {
        private const string CMD_getBuyResetTimes = "LongMarch.getBuyResetTimes";
        private const string CMD_getFinishedReward = "LongMarch.getFinishedReward";
        private const string CMD_getHeroStatus = "LongMarch.getHeroStatus";
        private const string CMD_getMyStatus = "LongMarch.getMyStatus";
        private const string CMD_getUnpassBuff = "LongMarch.getUnpassBuff";
        private const string CMD_restart = "LongMarch.restart";

        public static RequestReturnObject getBuyResetTimes(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getBuyResetTimes);
        }

        public static RequestReturnObject getFinishedReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getFinishedReward);
        }

        public static RequestReturnObject getHeroStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getHeroStatus);
        }

        public static RequestReturnObject getMyStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getMyStatus);
        }

        public static RequestReturnObject getUnpassBuff(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getUnpassBuff);
        }

        public static RequestReturnObject restart(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_restart);
        }


    }
}
