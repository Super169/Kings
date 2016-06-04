using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class DaqiaoActivity
    {
        private const string CMD_dailyRewardInfo = "DaqiaoActivity.dailyRewardInfo";
        private const string CMD_getCurrActivityInfo = "DaqiaoActivity.getCurrActivityInfo";
        private const string CMD_getDailyReward = "DaqiaoActivity.getDailyReward";

        public static RequestReturnObject dailyRewardInfo(HTTPRequestHeaders oH, string sid, int type)
        {
            string body = string.Format("{{\"type\":{0}}}", type);
            return com.SendGenericRequest(oH, sid, CMD_dailyRewardInfo, true, body);
        }

        public static RequestReturnObject getCurrActivityInfo(HTTPRequestHeaders oH, string sid, int type)
        {
            string body = string.Format("{{\"type\":{0}}}", type);
            return com.SendGenericRequest(oH, sid, CMD_getCurrActivityInfo, true, body);
        }

        public static RequestReturnObject getDailyReward(HTTPRequestHeaders oH, string sid, int type, int day)
        {
            string body = string.Format("{{\"type\":{0},\"day\":{1}}}", type, day);
            return com.SendGenericRequest(oH, sid, CMD_getDailyReward, true, body);
        }

    }
}
