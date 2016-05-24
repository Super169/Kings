using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static bool acceptArenaRankReward(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            RequestReturnObject rro;
            rro = request.Arena.getDefFormation(oH, sid);
            // No need to check error here, call getDefFormation is just follow the game flow, the data returned is not used

            rro = request.Arena.myArenaStatus(oH, sid);
            if (!rro.SuccessWithJson("rwdRank")) return false;
            int rwdRank = JSON.getInt(rro.responseJson, "rwdRank", 0);
            if (rwdRank == 0) return false;

            rro = request.Arena.acceptRankReward(oH, sid);
            if (!rro.SuccessWithJson("rwdRank")) return false;
            int endRwdRank = JSON.getInt(rro.responseJson, "rwdRank", -1);
            if (endRwdRank <= 0) return false;
            if (updateInfo != null) updateInfo(string.Format("{0}天下比武: 領取 {1}名的獎勵", oGA.msgPrefix, rwdRank));
            return true;
        }
    }
}
