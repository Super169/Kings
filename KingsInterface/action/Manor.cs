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
        public static List<ManorInfo> goGetManorInfo(HTTPRequestHeaders oH, string sid)
        {
            List<ManorInfo> manorInfo = new List<ManorInfo>();
            RequestReturnObject rro = go_Manor_getManorInfo(oH, sid);
            try
            {
                if ((rro.success) && (rro.responseJson["buildings"] != null))
                {
                    DynamicJsonArray buildings = rro.responseJson["buildings"];
                    foreach (dynamic building in buildings)
                    {
                        ManorInfo mi = new ManorInfo();
                        mi.field = building["field"];
                        mi.type = building["type"];
                        mi.level = building["level"];
                        mi.levelSeconds = building["levelSeconds"];
                        mi.heroIndex = building["heroIndex"];
                        mi.leftSeconds = building["leftSeconds"];
                        mi.products = building["products"];
                        mi.produceSeconds = building["produceSeconds"];
                        manorInfo.Add(mi);
                    }
                }
            }
            catch (Exception)
            {

            }
            return manorInfo;
        }

        public static int goHarvestProduct(HTTPRequestHeaders oH, string sid, int field, DelegateUpdateInfo updateInfo = null)
        {
            // No special handle required
            RequestReturnObject rro = go_Manor_harvestProduct(oH, sid, field);
            if (!rro.success)
            {
                if (updateInfo != null) updateInfo("收取資源失敗");
                return -1;
            }
            if ((rro.responseJson == null) || (rro.responseJson["out"] == null))
            {
                return 0;
            }
            int outProduct = getInt(rro.responseJson, "out");
            return outProduct;
        }

        public static bool goHarvestAll(HTTPRequestHeaders oH, string sid, DelegateUpdateInfo updateInfo = null)
        {
            PlayerProperties pp = action.goGetPlayerProperties(oH, sid);
            if (! pp.ready)
            {
                if (updateInfo != null) updateInfo("讀取主公資料失敗");
                return false;
            }

            if ((pp.FOOD >= pp.MAX_FOOD) && (pp.SILVER >= pp.MAX_SILVER) && (pp.IRON >= pp.MAX_IRON))
            {
                if (updateInfo != null) updateInfo("主公各項資源都滿了");
                return false;
            }

            List<ManorInfo> mis = action.goGetManorInfo(oH, sid);
            if (mis.Count == 0)
            {
                if (updateInfo != null) updateInfo("找不到封田資料");
                return false;
            }
            int getSILVER = 0, getFOOD = 0, getIRON = 0;

            foreach (ManorInfo mi in mis)
            {
                int outProducts = 0;
                switch (mi.type)
                {
                    case "MH":
                    case "SP":
                        if ((mi.products > 1000) && (pp.SILVER < pp.MAX_SILVER))
                        {
                            outProducts = action.goHarvestProduct(oH, sid, mi.field, updateInfo);
                            if (outProducts > 0)
                            {
                                // updateInfo(string.Format("收取 {0} 的銀 {1}", mi.field, mi.products));
                                getSILVER += outProducts;
                                pp.SILVER += outProducts;
                            }
                        }
                        break;
                    case "NT":
                    case "MC":
                        if ((mi.products > 1000) && (pp.FOOD < pp.MAX_FOOD))
                        {
                            outProducts = action.goHarvestProduct(oH, sid, mi.field, updateInfo);
                            if (outProducts > 0)
                            {
                                // updateInfo(string.Format("收取 {0} 的糧 {1}", mi.field, mi.products));
                                getFOOD += outProducts;
                                pp.FOOD += outProducts;
                            }
                        }
                        break;
                    case "LTC":
                        if ((mi.products > 1000) && (pp.IRON < pp.MAX_IRON))
                        {
                            outProducts = action.goHarvestProduct(oH, sid, mi.field, updateInfo);
                            if (outProducts > 0)
                            {
                                // updateInfo(string.Format("收取 {0} 的鐵 {1}", mi.field, mi.products));
                                getIRON += outProducts;
                                pp.IRON += outProducts;
                            }
                        }
                        break;
                }
            }
            if (updateInfo != null) updateInfo(string.Format("封地收獲: {0} 銀, {1} 糧, {2} 鐵", getSILVER, getFOOD, getIRON));

            return true;
        }

    }

}
