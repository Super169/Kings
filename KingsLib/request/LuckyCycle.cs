using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class LuckyCycle
    {
        private const string CMD_draw = "LuckyCycle.draw";
        private const string CMD_info = "LuckyCycle.info";

        public static RequestReturnObject draw(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_draw);
        }

        public static RequestReturnObject info(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_info);
        }

    }
}
