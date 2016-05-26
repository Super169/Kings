using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class BaiGuanYu
    {
        private const string CMD_getTimeInfo = "BaiGuanYu.getTimeInfo";

        public static RequestReturnObject getTimeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTimeInfo);
        }

    }
}
