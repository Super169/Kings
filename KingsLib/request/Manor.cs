using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Manor
    {
        private const string CMD_appointHero = "Manor.appointHero";
        private const string CMD_appointInfo = "Manor.appointInfo";
        private const string CMD_armsTechnology = "Manor.armsTechnology";
        private const string CMD_autoAppoint = "Manor.autoAppoint";
        private const string CMD_currIncEffect = "Manor.currIncEffect";
        private const string CMD_decreeInfo = "Manor.decreeInfo";
        private const string CMD_doHeroActivity = "Manor.doHeroActivity";
        private const string CMD_getManorInfo = "Manor.getManorInfo";
        private const string CMD_harvestActivity = "Manor.harvestActivity";
        private const string CMD_harvestProduct = "Manor.harvestProduct";
        private const string CMD_heroScoreTips = "Manor.heroScoreTips";
        private const string CMD_moveBuilding = "Manor.moveBuilding";
        private const string CMD_produceArmamentInfo = "Manor.produceArmamentInfo";
        private const string CMD_refreshManor = "Manor.refreshManor";
        private const string CMD_resHourOutput = "Manor.resHourOutput";
        private const string CMD_retireAll = "Manor.retireAll";
        private const string CMD_trainHeroInfo = "Manor.trainHeroInfo";
        private const string CMD_upgradeArmsTechnology = "Manor.upgradeArmsTechnology";

        public static RequestReturnObject armsTechnology(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_armsTechnology);
        }

        public static RequestReturnObject decreeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_decreeInfo);
        }

        public static RequestReturnObject getManorInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getManorInfo);
        }

        public static RequestReturnObject refreshManor(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_refreshManor);
        }

        public static RequestReturnObject resHourOutput(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_resHourOutput);
        }

    }
}
