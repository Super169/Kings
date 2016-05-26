using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class BossWar
    {
        private const string CMD_bossInfo = "BossWar.bossInfo ";
        private const string CMD_bossLineup = "BossWar.bossLineup ";
        private const string CMD_enterWar = "BossWar.enterWar ";
        private const string CMD_keyKillInfo = "BossWar.keyKillInfo ";
        private const string CMD_leaveWar = "BossWar.leaveWar ";
        private const string CMD_openInfo = "BossWar.openInfo ";
        private const string CMD_pk = "BossWar.pk ";
        private const string CMD_rankInfo = "BossWar.rankInfo ";
        private const string CMD_sendTroop = "BossWar.sendTroop ";

        public static RequestReturnObject bossInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_bossInfo);
        }

        public static RequestReturnObject bossLineup(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_bossLineup);
        }

        public static RequestReturnObject enterWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_enterWar);
        }

        public static RequestReturnObject keyKillInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_keyKillInfo);
        }

        public static RequestReturnObject leaveWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_leaveWar);
        }

        public static RequestReturnObject openInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_openInfo);
        }

        public static RequestReturnObject pk(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_pk);
        }

        public static RequestReturnObject rankInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_rankInfo);
        }




    }
}
