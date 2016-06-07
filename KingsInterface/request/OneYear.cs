using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class OneYear
    {
        private const string CMD_cityStatus = "OneYear.cityStatus";
        private const string CMD_info = "OneYear.info";

        public static RequestReturnObject cityStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_cityStatus);
        }
        public static RequestReturnObject info(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_info);
        }

    }
}
