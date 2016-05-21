using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class LordGodUp
    {
        private const string CMD_getDispInfo = "LordGodUp.getDispInfo";

        public static RequestReturnObject getDispInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getDispInfo);
        }

    }
}
