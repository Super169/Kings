using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Gamble
    {
        private const string CMD_chouqianOpenInfo = "Gamble.chouqianOpenInfo";

        public static RequestReturnObject chouqianOpenInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_chouqianOpenInfo);
        }

    }
}
