using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Login
    {
        private const string CMD_getOfflineConpensate = "Login.getOfflineConpensate";
        private const string CMD_login = "Login.login";
        private const string CMD_loginFinish = "Login.loginFinish";
        private const string CMD_serverInfo = "Login.serverInfo";

        public static RequestReturnObject getOfflineConpensate(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getOfflineConpensate);
        }

        public static RequestReturnObject loginFinish(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_loginFinish);
        }

        public static RequestReturnObject serverInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_serverInfo);
        }


    }
}
