using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class RewardActivity
    {
        private const string CMD_getSevenDayFundRewardInfo = "RewardActivity.getSevenDayFundRewardInfo";

        public static RequestReturnObject getSevenDayFundRewardInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getSevenDayFundRewardInfo);
        }


    }
}
