using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class Campaign
    {
        private const string CMD_eliteBuyTime = "Campaign.eliteBuyTime";
        private const string CMD_eliteFight = "Campaign.eliteFight";
        private const string CMD_eliteGetAllInfos = "Campaign.eliteGetAllInfos";
        private const string CMD_eliteGetCampaignInfo = "Campaign.eliteGetCampaignInfo";
        private const string CMD_evalMarchFormation = "Campaign.evalMarchFormation";
        private const string CMD_fightNext = "Campaign.fightNext";
        private const string CMD_getAttFormation = "Campaign.getAttFormation";
        private const string CMD_getLeftTimes = "Campaign.getLeftTimes";
        private const string CMD_getTrialsInfo = "Campaign.getTrialsInfo";
        private const string CMD_nextEnemies = "Campaign.nextEnemies";
        private const string CMD_quitCampaign = "Campaign.quitCampaign";
        private const string CMD_saveFormation = "Campaign.saveFormation";
        private const string CMD_trialsBuyTimes = "Campaign.trialsBuyTimes";

        public static RequestReturnObject eliteBuyTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_eliteBuyTime);
        }

        public static RequestReturnObject eliteGetAllInfos(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_eliteGetAllInfos);
        }

        public static RequestReturnObject fightNext(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_fightNext);
        }

        public static RequestReturnObject getAttFormation(HTTPRequestHeaders oH, string sid)
        {
            string body = "{\"march\":\"TRAVEL\"}";
            return com.SendGenericRequest(oH, sid, CMD_getAttFormation, true, body);
        }
        
        public static RequestReturnObject getLeftTimes(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getLeftTimes);
        }

        public static RequestReturnObject getTrialsInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTrialsInfo);
        }

        public static RequestReturnObject nextEnemies(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_nextEnemies);
        }

        public static RequestReturnObject saveFormation(HTTPRequestHeaders oH, string sid, string body)
        {
            return com.SendGenericRequest(oH, sid, CMD_saveFormation, true, body);
        }

        public static RequestReturnObject quitCampaign(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_quitCampaign);
        }

        
    }
}
