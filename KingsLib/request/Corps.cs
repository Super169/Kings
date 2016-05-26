using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Corps
    {
        private const string CMD_corpsCityReward = "Corps.corpsCityReward";
        private const string CMD_getCityRewardInfo = "Corps.getCityRewardInfo";
        private const string CMD_getCorpsJoinedPlayers = "Corps.getCorpsJoinedPlayers";
        private const string CMD_getCorpsMessageNum = "Corps.getCorpsMessageNum";
        private const string CMD_getJoinedCorps = "Corps.getJoinedCorps";
        private const string CMD_getNationalRank = "Corps.getNationalRank";
        private const string CMD_personIndustryList = "Corps.personIndustryList";
        private const string CMD_personIndustryRefresh = "Corps.personIndustryRefresh";
        private const string CMD_playerTech = "Corps.playerTech";
        private const string CMD_takeZhanjiStep = "Corps.takeZhanjiStep";
        private const string CMD_upgradePlayerTech = "Corps.upgradePlayerTech";

        public static RequestReturnObject corpsCityReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_corpsCityReward);
        }

        public static RequestReturnObject getCorpsMessageNum(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getCorpsMessageNum);
        }

        public static RequestReturnObject getJoinedCorps(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getJoinedCorps);
        }

        public static RequestReturnObject personIndustryRefresh(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_personIndustryRefresh);
        }

        public static RequestReturnObject playerTech(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_playerTech);
        }

        public static RequestReturnObject takeZhanjiStep(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_takeZhanjiStep);
        }




    }
}
