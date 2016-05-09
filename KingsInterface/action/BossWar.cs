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
        public static BossWarInfo goBossWarOnce(HTTPRequestHeaders oH, string sid, string body, DelegateUpdateInfo updateInfo)
        {
            BossWarInfo bwi = new BossWarInfo();
            RequestReturnObject rro;
            rro = go_BossWar_enterWar(oH, sid);
            bwi.enterFail = !rro.success;
            // Nothing to do if fail to enter
            if (bwi.enterFail) return bwi;
            
            if ((rro.responseJson == null) || (rro.style == "ERROR")) 
            {
                bwi.bossAvailable = false;
                return bwi;
            }
            bwi.beforeCnt = getInt(rro.responseJson, "sendCount", -1);
            if ((rro.responseJson["bossInfo"] != null) && (rro.responseJson["bossInfo"]["hpp"] != null))
            {
                bwi.bossHP = getInt(rro.responseJson["bossInfo"], "hpp", -1);
            }

            rro = go_BossWar_sendTroop(oH, sid, body);
            bwi.sendFail = (rro.ok != 1);
            if (bwi.sendFail)
            {
                bwi.sendFail = true;
            }

            rro = go_BossWar_leaveWar(oH, sid);
            bwi.leavelFail = !rro.success;

            return bwi;
        }
    }
}
