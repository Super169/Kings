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
        public static int goEliteBuyTime(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            RequestReturnObject rro;
            rro = request.Campaign.getLeftTimes(oH, sid);
            if (!rro.SuccessWithJson("eliteBuyTimes")) return 0;
            int eliteBuyTimes = JSON.getInt(rro.responseJson, "eliteBuyTimes");
            if (eliteBuyTimes > 0) return 0;
            rro = request.Campaign.eliteBuyTime(oH, sid);
            if (!rro.SuccessWithJson("eliteBuyTimes")) return 0;
            eliteBuyTimes = JSON.getInt(rro.responseJson, "eliteBuyTimes");
            if ((eliteBuyTimes > 0) && (updateInfo != null)) updateInfo(string.Format("{0}討伐: 購買{1}次", oGA.msgPrefix, eliteBuyTimes));
            return eliteBuyTimes;
        }

        public static int goTrialsBuyTimes(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            int buyCnt = 0;
            RequestReturnObject rro;
            rro = request.Campaign.getTrialsInfo(oH, sid);
            if (!rro.SuccessWithJson("weekday") || !rro.SuccessWithJson("buyTimes", typeof(DynamicJsonObject))) return 0;
            int weekday = JSON.getInt(rro.responseJson["weekday"]);
            dynamic buyTimes = rro.responseJson["buyTimes"];
            string[] trialType = { "", "WZLJ", "WJDD", "WHSJ" };
            for (int idx = 1; idx <= 3; idx++)
            {
                if ((weekday == 0) || (weekday == idx) || (weekday == idx + 3))
                {
                    int buyTime = JSON.getInt(buyTimes, trialType[idx]);
                    if (buyTime == 0)
                    {
                        rro = request.Campaign.trialsBuyTimes(oH, sid, trialType[idx]);
                        if (rro.ok == 1) buyCnt++;
                    }
                }
            }
            if ((buyCnt > 0) && (updateInfo != null)) updateInfo(string.Format("{0}試練: 購買{1}次", oGA.msgPrefix, buyCnt));
            return buyCnt;
        }

    }
}
