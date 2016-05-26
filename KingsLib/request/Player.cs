using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Player
    {
        private const string CMD_getProperties = "Player.getProperties";
        private const string CMD_getSpecialState = "Player.getSpecialState";
        private const string CMD_updateGuideSequence = "Player.updateGuideSequence";

        public static RequestReturnObject getProperties(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getProperties);
        }

        public static RequestReturnObject getSpecialState(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getSpecialState);
        }


    }
}
