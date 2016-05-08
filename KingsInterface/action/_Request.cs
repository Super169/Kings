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
        #region "Archery"

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

        #endregion "Archery"

        #region "BossWar"

        public static RequestReturnObject go_BossWar_enterWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_BossWar_enterWar);
        }

        public static RequestReturnObject go_BossWar_leaveWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_BossWar_leaveWar);
        }

        public static RequestReturnObject go_BossWar_sendTroop(HTTPRequestHeaders oH, string sid, string body)
        {
            return com.SendGenericRequest(oH, sid, CMD_BossWar_sendTroop, true, body);
        }

        #endregion

        #region "Hero"


        public static RequestReturnObject go_Hero_getPlayerHeroList(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Hero_getPlayerHeroList);
        }

        #endregion "Hero"

        #region "Manor"

        public static RequestReturnObject go_Manor_decreeInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Manor_decreeInfo);
        }

        public static RequestReturnObject go_Manor_getManorInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Manor_getManorInfo);
        }

        public static RequestReturnObject go_Manor_harvestProduct(HTTPRequestHeaders oH, string sid, int field)
        {
            string body = "{\"field\":" + field.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Manor_harvestProduct, true, body);
        }

        #endregion "Manor"

        #region "Player"

        public static RequestReturnObject go_Player_getProperties(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Player_getProperties);
        }

        #endregion "Player"
        
        #region "Login"

        public static RequestReturnObject go_Login_login(HTTPRequestHeaders oH, string sid)
        {
            string body = string.Format("{{\"type\":\"WEB_BROWSER\", \"loginCode\":\"{0}\"}}", sid);
            return com.SendGenericRequest(oH, sid, CMD_Login_login, false, body);
        }

        #endregion "Login"
        
        #region "SignInReward"

        public static RequestReturnObject go_SignInReward_getInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_SignInReward_getInfo);
        }

        public static RequestReturnObject go_SignInReward_signIn(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_SignInReward_signIn);
        }

        public static RequestReturnObject go_SignInReward_signInMultiple(HTTPRequestHeaders oH, string sid, int signNum)
        {
            string body = string.Format("{{\"signNum\":{0}}}", signNum);
            return com.SendGenericRequest(oH, sid, CMD_SignInReward_signInMultiple, true, body);
        }

        #endregion "SignInReward"

        #region "Shop"

        public static RequestReturnObject go_Shop_getCycleShopInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Shop_getCycleShopInfo);
        }

        #endregion "Shop"



    }
}
