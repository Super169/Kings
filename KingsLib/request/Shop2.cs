using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Shop2
    {
        private const string CMD_availableShops = "Shop2.availableShops";
        private const string CMD_buyItem = "Shop2.buyItem";
        private const string CMD_shop2Info = "Shop2.shop2Info";

        public static RequestReturnObject availableShops(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_availableShops);
        }

        public static RequestReturnObject shop2Info(HTTPRequestHeaders oH, string sid, string shop2Type)
        {
            string body = "{\"shop2Type\":\"" + shop2Type + "\"}";
            return com.SendGenericRequest(oH, sid, CMD_shop2Info, true, body);
        }

        public static RequestReturnObject buyItem(HTTPRequestHeaders oH, string sid, int id, string shop2Type)
        {
            string body = "{\"id\":" + id.ToString() + ", \"shop2Type\":\"" + shop2Type + "\"}";
            return com.SendGenericRequest(oH, sid, CMD_buyItem, true, body);
        }


    }
}
