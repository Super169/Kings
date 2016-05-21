using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Bag
    {
        private const string CMD_getBagInfo = "Bag.getBagInfo";
        private const string CMD_useItem = "Bag.useItem";

        public static RequestReturnObject getBagInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getBagInfo);
        }


    }
}
