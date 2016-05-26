using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Vip
    {
        private const string CMD_firstChargeInfo = "Vip.firstChargeInfo";
        private const string CMD_monthCard = "Vip.monthCard";
        private const string CMD_vipPrivilegeLevel = "Vip.vipPrivilegeLevel";

        public static RequestReturnObject firstChargeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_firstChargeInfo);
        }

        public static RequestReturnObject monthCard(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_monthCard);
        }


    }
}
