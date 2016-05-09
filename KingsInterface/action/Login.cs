using Fiddler;
using KingsInterface.data;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static LoginInfo goGetAccountInfo(HTTPRequestHeaders oH, string sid)
        {
            LoginInfo info = new LoginInfo() { ready = false, sid = sid };
            RequestReturnObject rro = go_Login_login(oH, sid);
            if (!rro.SuccessWithJson("account")) return info;
            info.account = rro.responseJson.account;
            info.serverTitle = rro.responseJson.serverTitle;
            info.nickName = rro.responseJson.nickName;
            rro = com.SendGenericRequest(oH, sid, CMD_Player_getProperties);
            if (!rro.SuccessWithJson("pvs")) return info;
            // only assign the sid here if all data is ready. or should it use other field like isReady?
            DynamicJsonArray pvs = (DynamicJsonArray) rro.responseJson.pvs;
            foreach (dynamic j in pvs)
            {
                if (j.p == "CORPS_NAME") info.CORPS_NAME = j.v;
                else if (j.p == "LEVEL") info.LEVEL = j.v;
                else if (j.p == "VIP_LEVEL") info.VIP_LEVEL = j.v;
            }
            info.ready = true;
            return info;
        }
    }
}
