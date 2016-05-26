using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class YunYou
    {
        private const string CMD_getYunYouInfo = "YunYou.getYunYouInfo";

        public static RequestReturnObject getYunYouInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getYunYouInfo);
        }

    }
}
