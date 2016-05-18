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
		public static List<BagInfo> goBagGetInfo(HTTPRequestHeaders oH, string sid)
        {
            List<BagInfo> bis = new List<BagInfo>();
            RequestReturnObject rro = action.go_Bag_getBagInfo(oH, sid);
            if (!rro.SuccessWithJson("items")) return bis;

            DynamicJsonArray items = rro.responseJson["items"];

			foreach (dynamic item in items)
            {
                BagInfo bi = new BagInfo();
                bi.idx = JSON.getInt(item, "idx");
                if (bi.idx >= 0)
                {
                    bi.nm = item["nm"];
                    bi.n = JSON.getInt(item, "n");
                    bi.us = JSON.getBool(item, "us");
                    bis.Add(bi);
                }
            }
            return bis;
        }

		public static int goBagCleanUp(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            if (!oGA.IsOnline()) return 0;

            List<BagInfo> bis = goBagGetInfo(oGA.currHeader, oGA.sid);
            if (bis.Count == 0) return 0;

            int cleanUpCount = 0;

			foreach (BagInfo bi in bis)
            {
				if (bi.AutoUseItem())
                {
                    RequestReturnObject rro = action.go_Bag_useItem(oGA.currHeader, oGA.sid, bi.n, bi.idx);
                    if (rro.SuccessWithJson("deleted"))
                    {
                        cleanUpCount++;
                        if (updateInfo != null) updateInfo(oGA.msgPrefix() + string.Format("倉庫中 {0} 個 {1} 全部使用了", bi.n, bi.nm));
                    } else if (rro.SuccessWithJson("updated"))
                    {
                        cleanUpCount++;
                        if (updateInfo != null) updateInfo(oGA.msgPrefix() + string.Format("倉庫中使用了 {0} 個 {1}", bi.n, bi.nm));
                    }
                    // nothing can do even failed or anything not match
                }
            }
            return cleanUpCount;
        }
    }
}
