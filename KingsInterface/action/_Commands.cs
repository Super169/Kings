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
        public const string CMD_Archery_getArcheryInfo = "Archery.getArcheryInfo";
        public const string CMD_Archery_shoot = "Archery.shoot";

        public const string CMD_Login_login = "Login.login";

        public const string CMD_Hero_getPlayerHeroList = "Hero.getPlayerHeroList";

        public const string CMD_Player_getProperties = "Player.getProperties";

        public const string CMD_SignInReward_getInfo = "SignInReward.getInfo";
        public const string CMD_SignInReward_signIn = "SignInReward.signIn";
        public const string CMD_SignInReward_signInMultiple = "SignInReward.signInMultiple";

    }

}
