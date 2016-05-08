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
        public const string CMD_Archery_getArcheryInfo = "Archery.getArcheryInfo";
        public const string CMD_Archery_shoot = "Archery.shoot";

        public const string CMD_BossWar_enterWar = "BossWar.enterWar";
        public const string CMD_BossWar_leaveWar = "BossWar.leaveWar";
        public const string CMD_BossWar_sendTroop = "BossWar.sendTroop";

        public const string CMD_Login_login = "Login.login";

        public const string CMD_Hero_getPlayerHeroList = "Hero.getPlayerHeroList";

        public const string CMD_Manor_decreeInfo = "Manor.decreeInfo";
        public const string CMD_Manor_getManorInfo = "Manor.getManorInfo";
        public const string CMD_Manor_harvestProduct = "Manor.harvestProduct";

        public const string CMD_Player_getProperties = "Player.getProperties";

        public const string CMD_SignInReward_getInfo = "SignInReward.getInfo";
        public const string CMD_SignInReward_signIn = "SignInReward.signIn";
        public const string CMD_SignInReward_signInMultiple = "SignInReward.signInMultiple";

        public const string CMD_Shop_getCycleShopInfo = "Shop.getCycleShopInfo";


    }

}
