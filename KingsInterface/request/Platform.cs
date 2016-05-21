using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class Platform
    {
        private const string CMD_getPlatformInfo = "Platform.getPlatformInfo";

        public static RequestReturnObject getPlatformInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPlatformInfo);
        }

    }
}
