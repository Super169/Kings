using Fiddler;
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static LoginInfo getAccountInfo(HTTPRequestHeaders oH, string sid)
        {
            LoginInfo info = new LoginInfo() { ready = false, sid = sid };
            RequestReturnObject rro = request.Login.login(oH, sid);
            if (!rro.SuccessWithJson("account")) return info;
            info.account = JSON.getString(rro.responseJson, "account", "");
            info.serverTitle = JSON.getString(rro.responseJson, "serverTitle", "");
            info.nickName = JSON.getString(rro.responseJson, "nickName", "");
            rro = request.Player.getProperties(oH, sid);
            if (!rro.SuccessWithJson("pvs", typeof(DynamicJsonArray))) return info;
            // only assign the sid here if all data is ready. or should it use other field like isReady?
            DynamicJsonArray pvs = (DynamicJsonArray)rro.responseJson.pvs;
            foreach (dynamic j in pvs)
            {
                if (j.p == "CORPS_NAME") info.CORPS_NAME = JSON.getString(j, "v", "");
                else if (j.p == "LEVEL") info.LEVEL = JSON.getString(j, "v", "");
                else if (j.p == "VIP_LEVEL") info.VIP_LEVEL = JSON.getString(j, "v", "");
            }
            info.ready = true;
            return info;
        }

    }
}
