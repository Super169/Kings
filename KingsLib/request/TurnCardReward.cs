using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class TurnCardReward
    {
        private const string CMD_getTurnCardRewards = "TurnCardReward.getTurnCardRewards";
        private const string CMD_turnCard = "TurnCardReward.turnCard";

        public static RequestReturnObject getTurnCardRewards(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTurnCardRewards);
        }


    }
}
