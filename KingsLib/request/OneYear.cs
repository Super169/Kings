using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class OneYear
    {
        private const string CMD_cityStatus = "OneYear.cityStatus";

        public static RequestReturnObject cityStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_cityStatus);
        }


    }
}
