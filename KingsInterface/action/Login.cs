using Fiddler;
using KingsInterface.data;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static LoginInfo Login_login(HTTPRequestHeaders oH, string sid)
        {
            LoginInfo info = new LoginInfo() { ready = false, sid = sid };
            string sBody = string.Format("{{\"type\":\"WEB_BROWSER\", \"loginCode\":\"{0}\"}}", sid);
            RequestReturnObject rro = com.SendGenericRequest(oH, sid, CMD_Login_login, false, sBody);
            if (!rro.success) return info;
            dynamic json = com.getJsonFromResponse(rro.session);
            info.account = json.account;
            info.serverTitle = json.serverTitle;
            info.nickName = json.nickName;

            rro = com.SendGenericRequest(oH, sid, CMD_Player_getProperties);
            if (!rro.success) return info;
            // only assign the sid here if all data is ready. or should it use other field like isReady?
            json = com.getJsonFromResponse(rro.session);
            DynamicJsonArray pvs = (DynamicJsonArray)json.pvs;
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
