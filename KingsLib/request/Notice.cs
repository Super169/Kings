using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Notice
    {
        private const string CMD_queryAllMarqueeMessage = "Notice.queryAllMarqueeMessage";

        public static RequestReturnObject queryAllMarqueeMessage(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_queryAllMarqueeMessage);
        }


    }
}
