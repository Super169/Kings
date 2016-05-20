using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class Activity
    {
        private const string CMD_drawCompanyAnniversaryLoginReward = "Activity.drawCompanyAnniversaryLoginReward";
        private const string CMD_drawCompanyAnniversaryRechargeReward = "Activity.drawCompanyAnniversaryRechargeReward";
        private const string CMD_drawExchangeHoliday = "Activity.drawExchangeHoliday";
        private const string CMD_drawStrategicFundInfo = "Activity.drawStrategicFundInfo";
        private const string CMD_getActivityList = "Activity.getActivityList";
        private const string CMD_getAnnouncement = "Activity.getAnnouncement";
        private const string CMD_getBookHeroInfo = "Activity.getBookHeroInfo";
        private const string CMD_getCloudSellerInfo = "Activity.getCloudSellerInfo";
        private const string CMD_getPlayerGoBackActivityInfo = "Activity.getPlayerGoBackActivityInfo";
        private const string CMD_getRankInfo = "Activity.getRankInfo";
        private const string CMD_getRationActivity = "Activity.getRationActivity";
        private const string CMD_getRedPointActivityInfo = "Activity.getRedPointActivityInfo";
        private const string CMD_getShuangShiyiActivityInfo = "Activity.getShuangShiyiActivityInfo";
        private const string CMD_getTuanGouOpenInfo = "Activity.getTuanGouOpenInfo";
        private const string CMD_serverOpenTime = "Activity.serverOpenTime";
        private const string CMD_showIconForServerOpenActivity = "Activity.showIconForServerOpenActivity";

        public static RequestReturnObject drawCompanyAnniversaryLoginReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawCompanyAnniversaryLoginReward);
        }



    }
}
