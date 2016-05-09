using Fiddler;
using KingsInterface.data;
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
            RequestReturnObject rro = action.go_Shop_getCycleShopInfo(oH, sid);
            if (!rro.SuccessWithJson("items")) return csis;
            try
            {
                DynamicJsonArray items = rro.responseJson["items"];
                foreach (dynamic item in items)
                {
                    CycleShopInfo csi = new CycleShopInfo();
                    csi.pos = getInt(item, "pos");
                    csi.res = item["res"];
                    csi.nm = item["nm"];
                    csi.amount = getInt(item, "amount");
                    csi.sold = (bool) item["sold"];
                    csis.Add(csi);
                }
            } catch (Exception)  { }

            return csis;
        }

        public static bool goShopbuyCycleShopItem(HTTPRequestHeaders oH, string sid, int pos)
        {
            RequestReturnObject rro = action.go_Shop_buyCycleShopItem(oH, sid, pos);
            if (!rro.SuccessWithJson("pos") || (rro.style == "ERROR")) return false;
            int retPos = getInt(rro.responseJson, "pos", -1);
            return (retPos == pos);
        }

    }
}
