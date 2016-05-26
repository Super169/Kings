using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class NpcCorps
    {
        private const string CMD_getNpcWars = "NpcCorps.getNpcWars";

        public static RequestReturnObject getNpcWars(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getNpcWars);
        }


    }
}
