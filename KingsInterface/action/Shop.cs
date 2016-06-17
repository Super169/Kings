using Fiddler;
using KingsInterface.data;
using KingsInterface.request;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;


namespace KingsInterface
{
    public partial class action
    {
        public static List<CycleShopInfo> goShopGetCycleShopInfo(HTTPRequestHeaders oH, string sid)
        {

            List<CycleShopInfo> csis = new List<CycleShopInfo>();
            RequestReturnObject rro = request.Shop.getCycleShopInfo(oH, sid);
            if (!rro.SuccessWithJson("items", typeof(DynamicJsonArray))) return csis;
            try
            {
                DynamicJsonArray items = rro.responseJson["items"];
                foreach (dynamic item in items)
                {
                    CycleShopInfo csi = new CycleShopInfo();
                    csi.pos = JSON.getInt(item, "pos");
                    csi.res = item["res"];
                    csi.nm = item["nm"];
                    csi.amount = JSON.getInt(item, "amount");
                    csi.sold = (bool) item["sold"];
                    csis.Add(csi);
                }
            } catch (Exception)  { }

            return csis;
        }

        public static bool goShopbuyCycleShopItem(HTTPRequestHeaders oH, string sid, int pos)
        {
            RequestReturnObject rro = request.Shop.buyCycleShopItem(oH, sid, pos);
            if (!rro.SuccessWithJson("pos") || (rro.style == "ERROR")) return false;
            int retPos = JSON.getInt(rro.responseJson, "pos", -1);
            return (retPos == pos);
        }

        public static int goSLShopBuyFood(GameAccount oGA)
        {
            PlayerProperties pp = action.goGetPlayerProperties(oGA.currHeader, oGA.sid);
            if ((!pp.ready) || (pp.EXPLOIT < 92)) return 0;

            RequestReturnObject rro = Shop2.shop2Info(oGA.currHeader, oGA.sid, "SL_SHOP");
            if (!rro.SuccessWithJson("remainBuyCount")) return 0;

            int coins = pp.EXPLOIT;
            int remainCount = JSON.getInt(rro.responseJson, "remainBuyCount");
            int buyCount = 0;
            bool error = false;

            while ((!error) && (coins >= 92) && (remainCount > 0))
            {
                rro = Shop2.buyItem(oGA.currHeader, oGA.sid, 1, 1, "SL_SHOP");
                error = (rro.ok != 1);
                if (!error)
                {
                    buyCount++;
                    remainCount--;
                    coins += 92;
                }
            }

            return buyCount;
        }

        public static bool goMarketTask(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            RequestReturnObject rro;
            rro = request.Task.getTaskTraceInfo(oH, sid);
            if (!rro.SuccessWithJson("tasks", typeof(DynamicJsonArray))) return false;
            DynamicJsonArray tasks = (DynamicJsonArray)rro.responseJson["tasks"];
            dynamic o = tasks.FirstOrDefault(x => JSON.getInt(x,"id") == 8);
            if (o == null) return false;
            if (JSON.getString(o, "status", "") != "ACC") return false;

            string buyType = "IRON";

            rro = request.Shop.tradeResource(oH, sid, buyType);
            if ((!rro.SuccessWithJson("resource")) ||
                (JSON.getString(rro.responseJson, "resource", "") != buyType))
            {
                // Try to buy food if IRON is not available
                buyType = "FOOD";
                rro = request.Shop.tradeResource(oH, sid, buyType);
                if ((!rro.SuccessWithJson("resource")) ||
                    (JSON.getString(rro.responseJson, "resource", "") != buyType))
                {
                    return false;
                }
            }
            if (updateInfo != null) updateInfo(string.Format("{0}市集: 為任務購買 {1}", oGA.msgPrefix, buyType));

            return true;
        }

    }
}
