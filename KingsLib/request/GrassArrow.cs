using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class GrassArrow
    {
        private const string CMD_acquireGrassArrowInfo = "GrassArrow.acquireGrassArrowInfo";
        private const string CMD_doGrassArrowFight = "GrassArrow.doGrassArrowFight";
        private const string CMD_drawStageReward = "GrassArrow.drawStageReward";
        private const string CMD_exchangeGrassArrow = "GrassArrow.exchangeGrassArrow";

        public static RequestReturnObject acquireGrassArrowInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_acquireGrassArrowInfo);
        }

        public static RequestReturnObject doGrassArrowFight(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_doGrassArrowFight);
        }


    }
}
