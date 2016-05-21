using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Drama
    {
        private const string CMD_getDramaInfo = "Drama.getDramaInfo";
        public static RequestReturnObject getDramaInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getDramaInfo);
        }

    }
}
