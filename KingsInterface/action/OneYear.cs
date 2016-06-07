using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {

        public static int goOneYearSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            string KEY_SignInType = "登陆";

            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            long currTime = action.getSystemTime(oH, sid) / 1000;
            RequestReturnObject rro;
            rro = request.OneYear.cityStatus(oH, sid);
            if (!rro.SuccessWithJson("activityStatus", typeof(DynamicJsonArray))) return 0;
            DynamicJsonArray activityStatus = rro.responseJson["activityStatus"];
            bool oneYearActivity = false;
            foreach (dynamic o in activityStatus)
            {
                if (JSON.exists(o, "startTime") && JSON.exists(o, "endTime"))
                {
                    long startTime = JSON.getLong(o, "startTime");
                    long endTime = JSON.getLong(o, "endTime");
                    if ((startTime < currTime) && (currTime < endTime))
                    {
                        oneYearActivity = true;
                        break;
                    }
                }
            }
            if (!oneYearActivity) return 0;

            rro = request.OneYearEntry.getOneYearEntryInfo(oH, sid);
            if (!rro.SuccessWithJson("entryLists", typeof(DynamicJsonArray))) return 0;

            DynamicJsonArray dja = (DynamicJsonArray)rro.responseJson["entryLists"];
            foreach(dynamic o in dja)
            {
                if (JSON.getString(o["type"]) == KEY_SignInType)
                {
                    bool isDraw = JSON.getBool(o, "isDraw", true);
                    if (!isDraw)
                    {
                        string rewardList = "";
                        if ((JSON.exists(o, "reward")) && JSON.exists(o["reward"]["itemDefs"],typeof(DynamicJsonArray))) {
                            DynamicJsonArray itemDefs = o["reward"]["itemDefs"];
                            foreach (dynamic i in itemDefs)
                            {
                                if (JSON.exists(i, "name") && JSON.exists(i,"num"))
                                {
                                    string name = JSON.getString(i, "name", null);
                                    int num = JSON.getInt(i, "num");
                                    rewardList += (rewardList == "" ? "" : ", ") + name + " x " + num.ToString();
                                }
                            }
                        }
                        rro = request.OneYearEntry.draw(oH, sid, KEY_SignInType);
                        if (rro.ok == 1) updateInfo(oGA.msgPrefix + "嘉年華活動: " + rewardList);
                    }
                }
            }

            return 0;
        }
    }
}
