using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class NationalWar
    {
        private const string CMD_acquireCityCommandInfo = "NationalWar.acquireCityCommandInfo";
        private const string CMD_acquireNationCardPanelInfo = "NationalWar.acquireNationCardPanelInfo";
        private const string CMD_cityDeclareInfo = "NationalWar.cityDeclareInfo";
        private const string CMD_convertNationCardReward = "NationalWar.convertNationCardReward";
        private const string CMD_getMyTroops = "NationalWar.getMyTroops";


        public static RequestReturnObject acquireCityCommandInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_acquireCityCommandInfo);
        }

        public static RequestReturnObject acquireNationCardPanelInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_acquireNationCardPanelInfo);
        }

        public static RequestReturnObject getMyTroops(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getMyTroops);
        }


    }
}
