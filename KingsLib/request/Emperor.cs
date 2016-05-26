using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Emperor
    {
        private const string CMD_collect = "Emperor.collect";
        private const string CMD_collected = "Emperor.collected";
        private const string CMD_draw = "Emperor.draw";
        private const string CMD_getBuyInfo = "Emperor.getBuyInfo";
        private const string CMD_getGameInfo = "Emperor.getGameInfo";
        private const string CMD_isFloat = "Emperor.isFloat";

        public static RequestReturnObject collected(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_collected);
        }

        public static RequestReturnObject draw(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_draw);
        }

        public static RequestReturnObject getBuyInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getBuyInfo);
        }

        public static RequestReturnObject getGameInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getGameInfo);
        }

        public static RequestReturnObject isFloat(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_isFloat);
        }



    }
}
