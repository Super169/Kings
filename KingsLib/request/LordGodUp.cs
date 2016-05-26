using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class LordGodUp
    {
        private const string CMD_getDispInfo = "LordGodUp.getDispInfo";

        public static RequestReturnObject getDispInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getDispInfo);
        }


    }
}
