using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class ZaJinDan
    {
        private const string CMD_getTimeInfo = "ZaJinDan.getTimeInfo";

        public static RequestReturnObject getTimeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTimeInfo);
        }

    }
}
