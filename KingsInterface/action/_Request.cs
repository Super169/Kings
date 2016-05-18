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

        #region "Bag"

        public static RequestReturnObject go_Bag_getBagInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Bag_getBagInfo);
        }

        public static RequestReturnObject go_Bag_useItem(HTTPRequestHeaders oH, string sid, int num, int index)
        {
            string body = "{\"paramList\":[\"-1\"],\"num\":" + num.ToString() + ",\"index\":" + index.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Bag_useItem, true, body);
        }

        #endregion


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

        #region "City"

        public static RequestReturnObject go_City_buyProduct(HTTPRequestHeaders oH, string sid, int industryId, int index)
        {
            string body = "{\"industryId\":" + industryId.ToString() + ", \"index\":" + index.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_City_buyProduct, true, body);
        }

        public static RequestReturnObject go_City_getIndustryInfo(HTTPRequestHeaders oH, string sid, int industryId)
        {
            string body = "{\"industryId\":" + industryId.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_City_getIndustryInfo, true, body);
        }

        #endregion

        #region "Corps"

        public static RequestReturnObject go_Corps_personIndustryList(HTTPRequestHeaders oH, string sid, string type)
        {
            string body = "{\"industryId\":\"" + type + "\"}";
            return com.SendGenericRequest(oH, sid, CMD_Corps_personIndustryList, true, body);
        }

        #endregion

        #region "Email"

        public static RequestReturnObject go_Email_getAttachment(HTTPRequestHeaders oH, string sid, int emailId)
        {
            string body = "{\"emailId\":" + emailId.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Email_getAttachment, true, body);
        }

        public static RequestReturnObject go_Email_openInBox(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Email_openInBox);
        }

        public static RequestReturnObject go_Email_read(HTTPRequestHeaders oH, string sid, int emailId)
        {
            string body = "{\"emailId\":" + emailId.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Email_read, true, body);
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

        public static RequestReturnObject go_Manor_refreshManor(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Manor_refreshManor);
        }



        #endregion "Manor"

        #region "Naval"

        public static RequestReturnObject go_Naval_enterWar(HTTPRequestHeaders oH, string sid, int n)
        {
            string body = "{\"n\":" + n.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Naval_enterWar, true, body);
        }

        public static RequestReturnObject go_Naval_getInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Naval_getInfo);
        }

        public static RequestReturnObject go_Naval_inMissionHeros(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Naval_inMissionHeros);
        }

        public static RequestReturnObject go_Naval_killRank(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Naval_killRank);
        }

        public static RequestReturnObject go_Naval_leaveWar(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Naval_leaveWar);
        }

        public static RequestReturnObject go_Naval_rewardCfg(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Naval_rewardCfg);
        }

        public static RequestReturnObject go_Naval_sendTroops(HTTPRequestHeaders oH, string sid, int cityId, string heros)
        {
            // string body = "{\"save\":" + heros + ",\"type\":\"NAVAL\",\"cityId\":" + cityId.ToString() + "}";
            string body = string.Format("{{\"save\":{0},\"type\":\"NAVAL\",\"cityId\":{1}}}", heros, cityId);
            
            return com.SendGenericRequest(oH, sid, CMD_Naval_sendTroops, true, body);
        }

        #endregion


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

        public static RequestReturnObject go_Shop_buyCycleShopItem(HTTPRequestHeaders oH, string sid, int pos)
        {
            string body = "{\"pos\":" + pos.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Shop_buyCycleShopItem, true, body);
        }

        #endregion "Shop"

        #region "Shop2"

        public static RequestReturnObject go_Shop2_availableShops(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Shop2_availableShops);
        }

        public static RequestReturnObject go_Shop2_shop2Info(HTTPRequestHeaders oH, string sid, string shop2Type)
        {
            string body = "{\"shop2Type\":\"" + shop2Type + "\"}";
            return com.SendGenericRequest(oH, sid, CMD_Shop2_shop2Info, true, body);
        }

        public static RequestReturnObject go_Shop2_buyItem(HTTPRequestHeaders oH, string sid, int id, string shop2Type)
        {
            string body = "{\"id\":" + id.ToString() + ", \"shop2Type\":\"" + shop2Type + "\"}";
            return com.SendGenericRequest(oH, sid, CMD_Shop2_buyItem, true, body);
        }



        #endregion


        #region "System"

        public static RequestReturnObject go_System_ping(HTTPRequestHeaders oH, string sid)
        {
            TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            Int64 jsTime = (Int64)(t.TotalMilliseconds + 0.5);
            string body = "{\"clientTime\":\"" + jsTime.ToString() + " \"}";
            return com.SendGenericRequest(oH, sid, CMD_System_ping, true, body);
        }

        #endregion


        #region "Task"

        public static RequestReturnObject go_Task_finishTask(HTTPRequestHeaders oH, string sid, int taskId)
        {
            string body = "{\"taskId\":" + taskId.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Task_finishTask, true, body);
        }

        public static RequestReturnObject go_Task_getTaskTraceInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Task_getTaskTraceInfo);
        }

        #endregion

        #region "Travel"

        public static RequestReturnObject go_Travel_getStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Travel_getStatus);
        }

        public static RequestReturnObject go_Travel_getMapInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Travel_getMapInfo);
        }

        public static RequestReturnObject go_Travel_controlDice(HTTPRequestHeaders oH, string sid, int num)
        {
            string body = "{\"num\":" + num.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Travel_controlDice, true, body);
        }

        public static RequestReturnObject go_Travel_dice(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Travel_dice);
        }

        public static RequestReturnObject go_Travel_viewStep(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_Travel_viewStep);
        }

        public static RequestReturnObject go_Travel_arriveStep(HTTPRequestHeaders oH, string sid, int step)
        {
            string body = "{\"step\":" + step.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_Travel_arriveStep, true, body);
        }

        #endregion


    }

}
