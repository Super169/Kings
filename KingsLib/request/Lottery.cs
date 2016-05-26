using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Lottery
    {
        private const string CMD_drawLottery = "Lottery.drawLottery";
        private const string CMD_freeLottery = "Lottery.freeLottery";
        private const string CMD_openLottery = "Lottery.openLottery";
        private const string CMD_refreshLottery = "Lottery.refreshLottery";

        public static RequestReturnObject drawLottery(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawLottery);
        }

        public static RequestReturnObject freeLottery(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_freeLottery);
        }

        public static RequestReturnObject openLottery(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_openLottery);
        }

        public static RequestReturnObject refreshLottery(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_refreshLottery);
        }

    }
}
