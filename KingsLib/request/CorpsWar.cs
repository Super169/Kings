using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class CorpsWar
    {
        private const string CMD_getInfo = "CorpsWar.getInfo";
        public static RequestReturnObject getInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getInfo);
        }


    }
}
