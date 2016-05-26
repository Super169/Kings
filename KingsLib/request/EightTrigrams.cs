using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class EightTrigrams
    {
        private const string CMD_attack = "EightTrigrams.attack";
        private const string CMD_getInfo = "EightTrigrams.getInfo";
        private const string CMD_open = "EightTrigrams.open";
        private const string CMD_openBox = "EightTrigrams.openBox";

        public static RequestReturnObject getInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getInfo);
        }

        public static RequestReturnObject open(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_open);
        }

        public static RequestReturnObject openBox(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_openBox);
        }

    }
}
