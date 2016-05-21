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
        private const string CMD_getShuangShiyiActivityReward = "Activity.getShuangShiyiActivityReward";
        private const string CMD_getTuanGouOpenInfo = "Activity.getTuanGouOpenInfo";
        private const string CMD_serverOpenTime = "Activity.serverOpenTime";
        private const string CMD_showIconForServerOpenActivity = "Activity.showIconForServerOpenActivity";

        public static RequestReturnObject drawCompanyAnniversaryLoginReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawCompanyAnniversaryLoginReward);
        }

        public static RequestReturnObject drawCompanyAnniversaryRechargeReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawCompanyAnniversaryRechargeReward);
        }

        public static RequestReturnObject drawExchangeHoliday(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawExchangeHoliday);
        }

        public static RequestReturnObject drawStrategicFundInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawStrategicFundInfo);
        }

        public static RequestReturnObject getActivityList(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getActivityList);
        }

        public static RequestReturnObject getAnnouncement(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getAnnouncement);
        }

        public static RequestReturnObject getBookHeroInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getBookHeroInfo);
        }

        public static RequestReturnObject getCloudSellerInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getCloudSellerInfo);
        }

        public static RequestReturnObject getPlayerGoBackActivityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPlayerGoBackActivityInfo);
        }

        public static RequestReturnObject getRankInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getRankInfo);
        }

        public static RequestReturnObject getRationActivity(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getRationActivity);
        }


        public static RequestReturnObject getShuangShiyiActivityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getShuangShiyiActivityInfo);
        }

        public static RequestReturnObject getShuangShiyiActivityReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getShuangShiyiActivityReward);
        }

        public static RequestReturnObject getTuanGouOpenInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTuanGouOpenInfo);
        }

        public static RequestReturnObject serverOpenTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_serverOpenTime);
        }

        public static RequestReturnObject showIconForServerOpenActivity(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_showIconForServerOpenActivity);
        }




    }
}
