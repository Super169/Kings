using Fiddler;
using KingsInterface.data;
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
        public static List<Industry> goGetIndustryList(GameAccount oGA, string type)
        {
            if (!oGA.IsOnline()) return null;
            List<Industry> industrys = new List<Industry>();
            try
            {
                RequestReturnObject rro = go_Corps_personIndustryList(oGA.currHeader, oGA.sid, type);
                if (!rro.SuccessWithJson("items")) return null;
                if (!(rro.responseJson["items"] is DynamicJsonArray)) return null;
                {
                    DynamicJsonArray items = rro.responseJson["items"];
                    foreach (dynamic item in items)
                    {
                        Industry industry = new Industry();
                        industry.industryId = JSON.getInt(item.industryId);
                        industry.city = JSON.getString(item.city);
                        industry.type = JSON.getString(item.type);
                        industrys.Add(industry);
                    }
                }
            }
            catch
            {
                industrys = null;
            }

            return industrys;
        }

        public static List<IndustryInfo> goGetIndustryInfo(GameAccount oGA, int industryId)
        {
            if (!oGA.IsOnline()) return null;
            List<IndustryInfo> iis = new List<IndustryInfo>();
            try
            {
                RequestReturnObject rro = go_City_getIndustryInfo(oGA.currHeader, oGA.sid, industryId);
                if (!rro.SuccessWithJson("items")) return iis;
                if (!(rro.responseJson["items"] is DynamicJsonArray)) return iis;
                DynamicJsonArray items = rro.responseJson["items"];
                foreach (dynamic item in items)
                {
                    IndustryInfo ii = new IndustryInfo();
                    ii.config = JSON.getInt(item, "config");
                    ii.discount = JSON.getInt(item, "discount");
                    ii.sold = JSON.getBool(item, "sold");
                    iis.Add(ii);
                }
            }
            catch
            {
                iis = new List<IndustryInfo>();
            }
            return iis;
        }

        public static int goIndustryBuyAll(GameAccount oGA, DelegateUpdateInfo updateInfo, bool buyFood, bool buySilver)
        {
            if (!oGA.IsOnline()) return 0;
            int buyCnt = 0;

            PlayerProperties pp = goGetPlayerProperties(oGA.currHeader, oGA.sid);
            if (!pp.ready) return 0;

            int exploit = pp.EXPLOIT;

            List<Industry> industrys = action.goGetIndustryList(oGA, "");
            if (industrys != null)
            {
                foreach (Industry industry in industrys)
                {
                    // UpdateResult(string.Format("{0}:{1}:{2}", ind.industryId, ind.city, ind.type));
                    if (industry.isMarket())
                    {
                        List<IndustryInfo> iis = goGetIndustryInfo(oGA, industry.industryId);
                        for (int idx = 0; idx < iis.Count; idx++)
                        {
                            IndustryInfo ii = iis.ElementAt(idx);
                            int itemCost = ii.ItemCost(exploit, buyFood, buySilver);
                            if (itemCost > 0)
                            {
                                RequestReturnObject rro = go_City_buyProduct(oGA.currHeader, oGA.sid, industry.industryId, idx);
                                if (rro.ok == 1)
                                {
                                    buyCnt++;
                                    exploit -= itemCost;
                                }

                            }
                        }
                    }
                }
            }
            if ((buyCnt > 0) && (updateInfo != null)) updateInfo(oGA.msgPrefix() + string.Format("在產業中購買了{0}次", buyCnt));
            return buyCnt;
        }


    }
}
