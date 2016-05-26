using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Arena
    {
        private const string CMD_acceptRankReward = "Arena.acceptRankReward";
        private const string CMD_changeEnemies = "Arena.changeEnemies";
        private const string CMD_getDefFormation = "Arena.getDefFormation";
        private const string CMD_myArenaStatus = "Arena.myArenaStatus";

        public static RequestReturnObject acceptRankReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_acceptRankReward);
        }

        public static RequestReturnObject changeEnemies(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_changeEnemies);
        }

        public static RequestReturnObject getDefFormation(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getDefFormation);
        }

        public static RequestReturnObject myArenaStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_myArenaStatus);
        }

    }
}
