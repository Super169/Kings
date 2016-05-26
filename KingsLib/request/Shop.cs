using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Shop
    {
        private const string CMD_availableShops = "Shop.availableShops";
        private const string CMD_buyCycleShopItem = "Shop.buyCycleShopItem";
        private const string CMD_buyShopItem = "Shop.buyShopItem";
        private const string CMD_buyTravelShopItem = "Shop.buyTravelShopItem";
        private const string CMD_getCycleShopInfo = "Shop.getCycleShopInfo";
        private const string CMD_getResourceTradeInfo = "Shop.getResourceTradeInfo";
        private const string CMD_getShopInfo = "Shop.getShopInfo";
        private const string CMD_getShuangShiyiShopInfo = "Shop.getShuangShiyiShopInfo";
        private const string CMD_getTravelShopInfo = "Shop.getTravelShopInfo";
        private const string CMD_otherShopsRefreshTime = "Shop.otherShopsRefreshTime";
        private const string CMD_refreshShop = "Shop.refreshShop";
        private const string CMD_shopNextRefreshTime = "Shop.shopNextRefreshTime";
        private const string CMD_tradeResource = "Shop.tradeResource";


        public static RequestReturnObject buyCycleShopItem(HTTPRequestHeaders oH, string sid, int pos)
        {
            string body = "{\"pos\":" + pos.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_buyCycleShopItem, true, body);
        }


        public static RequestReturnObject buyTravelShopItem(HTTPRequestHeaders oH, string sid, int idx)
        {
            string body = "{\"idx\":" + idx.ToString() + ", \"type\":\"TRAVEL\"}";
            return com.SendGenericRequest(oH, sid, CMD_buyTravelShopItem, true, body);
        }


        public static RequestReturnObject getCycleShopInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getCycleShopInfo);
        }

        public static RequestReturnObject getResourceTradeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getResourceTradeInfo);
        }


        public static RequestReturnObject getShuangShiyiShopInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getShuangShiyiShopInfo);
        }

        public static RequestReturnObject getTravelShopInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getTravelShopInfo);
        }

        public static RequestReturnObject otherShopsRefreshTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_otherShopsRefreshTime);
        }


        public static RequestReturnObject shopNextRefreshTime(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_shopNextRefreshTime);
        }

        public static RequestReturnObject tradeResource(HTTPRequestHeaders oH, string sid, string resource)
        {
            string body = string.Format("{{\"resouce\":\"{0}\"}}", resource);
            return com.SendGenericRequest(oH, sid, CMD_tradeResource, true, body);
        }

    }
}
