﻿using Fiddler;
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
        public static string goGenericAction(HTTPRequestHeaders oH, string sid, string command, bool addSId = true, string body = null)
        {
            string info = "";
            try
            {
                RequestReturnObject rro = com.SendGenericRequest(oH, sid, command, addSId, body);
                if (rro.success)
                {
                    info = com.GetResponseText(rro.session);
                }
                else
                {
                    info = rro.msg;
                }
            }
            catch (Exception ex)
            {
                info = ex.Message;
            }
            return info;
        }

    }

}
