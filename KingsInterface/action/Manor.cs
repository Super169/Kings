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
        private const int MIN_HARVEST = 100;

        public static List<DecreeInfo> goManorGetDecreeInfo(HTTPRequestHeaders oH, string sid)
        {
            List<DecreeInfo> decreeInfo = new List<DecreeInfo>();
            try
            {
                RequestReturnObject rro = go_Manor_decreeInfo(oH, sid);
                if ((rro.success) && (rro.responseJson != null) && (rro.responseJson["decHeros"] != null))
                {
                    DynamicJsonArray decHeros = rro.responseJson["decHeros"];

                    foreach (dynamic decree in decHeros)
                    {

                        DecreeInfo decInfo = new DecreeInfo() { decId = decree.decId };
                        DynamicJsonArray heros = decree.heros;
                        foreach (dynamic hero in heros)
                        {
                            int heroIdx = (hero.open ? hero.heroIdx : -1);
                            decInfo.heroIdx[hero.pos - 1] = heroIdx;
                            decInfo.heroName[hero.pos - 1] = (heroIdx > 0 ? "?" : (heroIdx == 0 ? "+" : "-"));
                        }
                        decreeInfo.Add(decInfo);

                    }
                }

            }
            catch (Exception) { }
            return decreeInfo;
        }

        public static List<DecreeInfo> goManorGetDecreeInfoWithName(HTTPRequestHeaders oH, string sid, List<HeroInfo> heroInfo)
        {
            List<DecreeInfo> decreeInfo = new List<DecreeInfo>();
            decreeInfo = goManorGetDecreeInfo(oH, sid);

            if ((heroInfo == null) && (heroInfo.Count == 0))
            {
                // Cannot get name withtou heroInfo
                return decreeInfo;
            }
            foreach (DecreeInfo di in decreeInfo)
            {
                for (int idx = 0; idx < 5; idx++)
                {
                    if (di.heroIdx[idx] > 0)
                    {
                        HeroInfo hi = heroInfo.SingleOrDefault(x => x.idx == di.heroIdx[idx]);
                        di.heroName[idx] = (hi == null ? "????" : hi.nm);
                    }
                }
            }
            return decreeInfo;
        }

        public static List<ManorInfo> goManorGetManorInfo(HTTPRequestHeaders oH, string sid)
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
            catch (Exception) { }

            return manorInfo;
        }

        public static int goManorHarvestProduct(HTTPRequestHeaders oH, string sid, int field, DelegateUpdateInfo updateInfo = null)
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

        public static bool goManorHarvestAll(GameAccount oGA, DelegateUpdateInfo updateInfo = null)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            PlayerProperties pp = action.goGetPlayerProperties(oH, sid);
            if (! pp.ready)
            {
                if (updateInfo != null) updateInfo(oGA.msgPrefix() + "讀取主公資料失敗");
                return false;
            }

            if ((pp.FOOD >= pp.MAX_FOOD) && (pp.SILVER >= pp.MAX_SILVER) && (pp.IRON >= pp.MAX_IRON))
            {
                if (updateInfo != null) updateInfo(oGA.msgPrefix() + "主公各項資源都滿了");
                return false;
            }

            List<ManorInfo> mis = action.goManorGetManorInfo(oH, sid);
            if (mis.Count == 0)
            {
                if (updateInfo != null) updateInfo(oGA.msgPrefix() + "找不到封田資料");
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
                        if ((mi.products > MIN_HARVEST) && (pp.SILVER < pp.MAX_SILVER))
                        {
                            outProducts = action.goManorHarvestProduct(oH, sid, mi.field, updateInfo);
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
                        if ((mi.products > MIN_HARVEST) && (pp.FOOD < pp.MAX_FOOD))
                        {
                            outProducts = action.goManorHarvestProduct(oH, sid, mi.field, updateInfo);
                            if (outProducts > 0)
                            {
                                // updateInfo(string.Format("收取 {0} 的糧 {1}", mi.field, mi.products));
                                getFOOD += outProducts;
                                pp.FOOD += outProducts;
                            }
                        }
                        break;
                    case "LTC":
                        if ((mi.products > MIN_HARVEST) && (pp.IRON < pp.MAX_IRON))
                        {
                            outProducts = action.goManorHarvestProduct(oH, sid, mi.field, updateInfo);
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
            if (updateInfo != null) updateInfo(string.Format("{0}封地收獲: {1} 銀, {2} 糧, {3} 鐵", oGA.msgPrefix(), getSILVER, getFOOD, getIRON));
            return true;
        }

    }

}
