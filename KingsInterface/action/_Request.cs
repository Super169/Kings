using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class action
    {
        public static RequestReturnObject go_Archery_getArcheryInfo(HTTPRequestHeaders oH, string sid)
        {
            string body = "{\"type\":\"NORMAL\"}";
            return com.SendGenericRequest(oH, sid, CMD_Archery_shoot, true, body);
        }

        public static RequestReturnObject go_Archery_shoot(HTTPRequestHeaders oH, string sid, int x, int y)
        {
            string body = "{\"x\":" + x.ToString() + ",\"y\":" + y.ToString() + ",\"type\":\"NORMAL\"}";
            return com.SendGenericRequest(oH, sid, CMD_Archery_shoot, true, body);
        }

        public static RequestReturnObject go_Hero_getPlayerHeroList(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Hero_getPlayerHeroList);
        }

        public static RequestReturnObject go_Login_login(HTTPRequestHeaders oH, string sid)
        {
            string body = string.Format("{{\"type\":\"WEB_BROWSER\", \"loginCode\":\"{0}\"}}", sid);
            return com.SendGenericRequest(oH, sid, CMD_Login_login, false, body);
        }

        public static RequestReturnObject go_SignInReward_getInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_SignInReward_getInfo);
        }

        public static RequestReturnObject go_SignInReward_signIn(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_SignInReward_signIn);
        }

    }
}
