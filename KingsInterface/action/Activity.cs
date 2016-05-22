using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static void goShuangShiyiActivityReward(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            RequestReturnObject rro;
            if (oGA.IsOnline())
            {
                rro = request.Activity.getShuangShiyiActivityReward(oGA.currHeader, oGA.sid);
                if (rro.ok == 1)
                {
                    rro = request.Activity.drawCompanyAnniversaryRechargeReward(oGA.currHeader, oGA.sid);
                    if (rro.success)
                    {
                        if (updateInfo != null) updateInfo(string.Format("{0}領取紅包", oGA.msgPrefix()));
                    }
                }
            }
        }

        public static void goTuanGou(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            RequestReturnObject rro = request.Activity.getTuanGouInfo(oGA.currHeader, oGA.sid);
            if (!rro.SuccessWithJson("bagInfos")) return;

            if (rro.responseJson["bagInfos"].GetType() != typeof(DynamicJsonArray)) return;
            DynamicJsonArray bagInfos = rro.responseJson["bagInfos"];

            if (rro.responseJson["rewardBags"].GetType() != typeof(DynamicJsonArray)) return;
            DynamicJsonArray rewardBags = rro.responseJson["rewardBags"];

            int sumSize = 0;
            foreach (dynamic o in bagInfos)
            {
                sumSize += JSON.getInt(o, "sumSize");
            }

            foreach (dynamic o in rewardBags)
            {
                int needSize = JSON.getInt(o, "needSize");
                bool isReward = JSON.getBool(o, "isReward");
                if ((needSize <= sumSize) && !isReward)
                {
                    int bagId = JSON.getInt(o, "bagId");
                    rro = request.Activity.tuanGouReward(oGA.currHeader, oGA.sid, bagId);
                    if (rro.ok == 1) {
                        if (updateInfo != null) updateInfo(string.Format("{0}跨服團購開啟寶箱 {1}", oGA.msgPrefix(), bagId));
                    }
                }
            }

        }
    }
}
