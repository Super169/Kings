using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class OperateActivity
    {
        private const string CMD_getUpgradeActivityInfo = "OperateActivity.getUpgradeActivityInfo";
        private const string CMD_upgradeActivityReward = "OperateActivity.upgradeActivityReward";

        public static RequestReturnObject getUpgradeActivityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getUpgradeActivityInfo);
        }

        public static RequestReturnObject upgradeActivityReward(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_upgradeActivityReward);
        }



    }
}
