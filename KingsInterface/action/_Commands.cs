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

        public const string CMD_BossWar_enterWar = "BossWar.enterWar";
        public const string CMD_BossWar_leaveWar = "BossWar.leaveWar";
        public const string CMD_BossWar_sendTroop = "BossWar.sendTroop";

        public const string CMD_Email_read = "Email.read";
        public const string CMD_Email_openInBox = "Email.openInBox";
        public const string CMD_Email_getAttachment = "Email.getAttachment";

        public const string CMD_Login_login = "Login.login";

        public const string CMD_Hero_getPlayerHeroList = "Hero.getPlayerHeroList";

        public const string CMD_Manor_decreeInfo = "Manor.decreeInfo";
        public const string CMD_Manor_getManorInfo = "Manor.getManorInfo";
        public const string CMD_Manor_harvestProduct = "Manor.harvestProduct";
        public const string CMD_Manor_refreshManor = "Manor.refreshManor";

        public const string CMD_Naval_enterWar = "Naval.enterWar";
        public const string CMD_Naval_getVersusCount = "Naval.getVersusCount";
        public const string CMD_Naval_getInfo = "Naval.getInfo";
        public const string CMD_Naval_inMissionHeros = "Naval.inMissionHeros";
        public const string CMD_Naval_killRank = "Naval.killRank";
        public const string CMD_Naval_leaveWar = "Naval.leaveWar";
        public const string CMD_Naval_rewardCfg = "Naval.rewardCfg";

        public const string CMD_Player_getProperties = "Player.getProperties";

        public const string CMD_SignInReward_getInfo = "SignInReward.getInfo";
        public const string CMD_SignInReward_signIn = "SignInReward.signIn";
        public const string CMD_SignInReward_signInMultiple = "SignInReward.signInMultiple";

        public const string CMD_Shop_getCycleShopInfo = "Shop.getCycleShopInfo";
        public const string CMD_Shop_buyCycleShopItem = "Shop.buyCycleShopItem";

        public const string CMD_Task_finishTask = "Task.finishTask";
        public const string CMD_Task_getTaskTraceInfo = "Task.getTaskTraceInfo";

    }

}
