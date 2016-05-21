using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Rank
    {
        private const string CMD_findAllPowerRank = "Rank.findAllPowerRank";

        public static RequestReturnObject findAllPowerRank(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_findAllPowerRank);
        }

    }
}
