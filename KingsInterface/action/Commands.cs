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
        public const string CMD_Login_login = "Login.login";

        public const string CMD_Hero_getPlayerHeroList = "Hero.getPlayerHeroList";

        public const string CMD_Player_getProperties = "Player.getProperties";

        public const string CMD_SignInReward_signIn = "SignInReward.signIn";


        // Generic method to test specified action
        public static RequestReturnObject goGenericAction(HTTPRequestHeaders oH, string sid, string command, bool addSId = true, string body = null)
        {
            RequestReturnObject rro;
            try
            {
                rro = com.SendGenericRequest(oH, sid, command, addSId, body);
            }
            catch (Exception ex)
            {
                rro = new RequestReturnObject() { success = false, msg = ex.Message };
            }
            return rro;
        }

    }

}
