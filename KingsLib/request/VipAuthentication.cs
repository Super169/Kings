using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class VipAuthentication
    {
        private const string CMD_isGotMobileGift = "VipAuthentication.isGotMobileGift";

        public static RequestReturnObject isGotMobileGift(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_isGotMobileGift);
        }


    }
}
