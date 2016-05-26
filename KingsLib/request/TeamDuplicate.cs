using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class TeamDuplicate
    {
        private const string CMD_teamDuplicateFreeTimes = "TeamDuplicate.teamDuplicateFreeTimes";

        public static RequestReturnObject teamDuplicateFreeTimes(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_teamDuplicateFreeTimes);
        }


    }
}
