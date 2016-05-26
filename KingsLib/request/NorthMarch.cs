using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class NorthMarch
    {
        private const string CMD_enterWar = "NorthMarch.enterWar";
        private const string CMD_inMissionHeros = "NorthMarch.inMissionHeros";
        private const string CMD_leaveWar = "NorthMarch.leaveWar";
        private const string CMD_northCitySituation = "NorthMarch.northCitySituation";
        private const string CMD_retreatAllTroops = "NorthMarch.retreatAllTroops";
        private const string CMD_sendTroops = "NorthMarch.sendTroops";

        public static RequestReturnObject enterWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_enterWar);
        }

        public static RequestReturnObject inMissionHeros(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_inMissionHeros);
        }

        public static RequestReturnObject leaveWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_leaveWar);
        }

        public static RequestReturnObject northCitySituation(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_northCitySituation);
        }

        public static RequestReturnObject retreatAllTroops(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_retreatAllTroops);
        }


    }
}
