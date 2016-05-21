using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Pray
    {
        private const string CMD_getPrayTime = "Pray.getPrayTime";

        public static RequestReturnObject getPrayTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPrayTime);
        }

    }
}
