using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class WelfareLottery
    {
        private const string CMD_time = "WelfareLottery.time";

        public static RequestReturnObject time(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_time);
        }

    }
}
